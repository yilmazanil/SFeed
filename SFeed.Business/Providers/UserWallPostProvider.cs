using SFeed.Core.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using SFeed.Core.Infrastructue.Repository;
using SFeed.SqlRepository;
using SFeed.RedisRepository;
using AutoMapper;
using SFeed.Core.Models.WallPost;

namespace SFeed.Business.Providers
{
    public class UserWallPostProvider : IUserWallPostProvider
    {
        private IRepository<WallPost> wallPostRepo;
        private ITypedCacheRepository<WallPostModel> wallPostCacheRepo;

        public UserWallPostProvider() : this(
            new RedisWallPostRepository(),
            new WallPostRepository())
        {

        }

        public UserWallPostProvider(
            ITypedCacheRepository<WallPostModel> wallPostCacheRepo,
            IRepository<WallPost> wallPostRepo)
        {
            this.wallPostCacheRepo = wallPostCacheRepo;
            this.wallPostRepo = wallPostRepo;

        }

        public string AddEntry(WallPostCreateRequest request)
        {
            var newPostId = Guid.NewGuid().ToString();
            var dbEntry = new WallPost
            {
                Body = request.Body,
                CreatedBy = request.PostedBy,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                Id = newPostId,
                PostType = Convert.ToByte(request.PostType),
                UserWall = new UserWall { UserId = request.WallOwnerId, WallPostId = newPostId }
            };

            //Save Post
            wallPostRepo.Add(dbEntry);
            wallPostRepo.CommitChanges();
            //Update Cache

            var cacheModel = Mapper.Map<WallPostModel>(request);
            cacheModel.Id = newPostId;
            cacheModel.PostType = (short)request.PostType;
            wallPostCacheRepo.AddItem(cacheModel);
            return newPostId;

        }

        public void UpdateEntry(WallPostModel model)
        {
            var existingEntry = wallPostRepo.Get(p => p.Id == model.Id);
            existingEntry.Body = model.Body;
            existingEntry.ModifiedDate = DateTime.Now;
            existingEntry.PostType = Convert.ToByte(model.PostType);

            //Update DB
            wallPostRepo.Update(existingEntry);
            wallPostRepo.CommitChanges();
            //Update Cache
            wallPostCacheRepo.UpdateItem(model.Id, model);
        }

        public void DeleteEntry(string postId)
        {
            //Mark as deleted
            wallPostRepo.Delete(p => p.Id == postId);
            wallPostRepo.CommitChanges();
            //Remove from Cache
            wallPostCacheRepo.RemoveItem(postId);

        }
        public WallPostModel GetEntry(string postId)
        {
            //Check cache
            var entry = wallPostCacheRepo.GetItem(postId);
            if (entry == null)
            {
                var dbEntry = wallPostRepo.Get(e => e.Id == postId && e.IsDeleted == false);

                if (dbEntry != null)
                {
                    entry = Mapper.Map<WallPostModel>(dbEntry);
                    //Update Cache
                    wallPostCacheRepo.AddItem(entry);
                }
            }
            return entry;
        }

        public IEnumerable<WallPostModel> GetUserWall(string wallOwnerId)
        {
            return wallPostRepo.GetMany(p => p.UserWall.UserId == wallOwnerId && p.IsDeleted == false).Select(p => new WallPostModel
            {
                Body = p.Body,
                PostedBy = p.CreatedBy,
                PostType = p.WallPostType.Id,
                Id = p.Id.ToString()
            });
        }

        public IEnumerable<WallPostModel> GetEntries(IEnumerable<string> postIds)
        {
            //Retrieve from cache
            return wallPostCacheRepo.GetByIds(postIds);
        }

        public void Dispose()
        {
            if (wallPostRepo != null)
            {
                wallPostRepo.Dispose();
            }

            if (wallPostCacheRepo != null)
            {
                wallPostCacheRepo.Dispose();
            }
        }

    }
}
