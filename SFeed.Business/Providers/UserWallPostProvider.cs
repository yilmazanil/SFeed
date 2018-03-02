using SFeed.Core.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using SFeed.Core.Models;
using SFeed.Core.Infrastructue.Repository;
using SFeed.SqlRepository;
using SFeed.RedisRepository;
using AutoMapper;

namespace SFeed.Business.Providers
{
    public class UserWallPostProvider : IUserWallPostProvider
    {
        private IRepository<WallPost> wallPostRepo;
        private ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo;

        public UserWallPostProvider() : this(
            new RedisWallEntryRepository(),
            new WallPostRepository())
        {

        }

        public UserWallPostProvider(
            ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo,
            IRepository<WallPost> wallPostRepo)
        {
            this.wallEntryCacheRepo = wallEntryCacheRepo;
            this.wallPostRepo = wallPostRepo;

        }

        public string AddEntry(WallEntryModel model, string wallOwnerId)
        {
            var newPostId = Guid.NewGuid().ToString();
            var dbEntry = new WallPost
            {
                Body = model.Body,
                CreatedBy = model.CreatedBy,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                Id = newPostId,
                PostType = Convert.ToByte(WallEntryTypeEnum.plaintext),
                UserWall = new UserWall { UserId = wallOwnerId, WallPostId = newPostId }
            };

            //Save Post
            wallPostRepo.Add(dbEntry);
            wallPostRepo.CommitChanges();
            //Update Cache
            model.Id = newPostId;
            wallEntryCacheRepo.AddItem(model);
            return newPostId.ToString();

        }

        public void UpdateEntry(WallEntryModel model)
        {
            var existingEntry = wallPostRepo.Get(p => p.Id == model.Id);
            existingEntry.Body = model.Body;
            existingEntry.ModifiedDate = DateTime.Now;

            //Update DB
            wallPostRepo.Update(existingEntry);
            wallPostRepo.CommitChanges();
            //Update Cache
            wallEntryCacheRepo.UpdateItem(model.Id, model);
        }

        public void DeleteEntry(string postId)
        {
            //Mark as deleted
            wallPostRepo.Delete(p => p.Id == postId);
            wallPostRepo.CommitChanges();
            //Remove from Cache
            wallEntryCacheRepo.RemoveItem(postId);

        }
        public WallEntryModel GetEntry(string postId)
        {
            //Check cache
            var entry = wallEntryCacheRepo.GetItem(postId);
            if (entry == null)
            {
                var dbEntry = wallPostRepo.Get(e => e.Id == postId && e.IsDeleted == false);

                if (dbEntry != null)
                {
                    entry = Mapper.Map<WallEntryModel>(dbEntry);
                    //Update Cache
                    wallEntryCacheRepo.AddItem(entry);
                }
            }
            return entry;
        }

        public IEnumerable<WallEntryModel> GetUserWall(string wallOwnerId)
        {
            return wallPostRepo.GetMany(p => p.UserWall.UserId == wallOwnerId && p.IsDeleted == false).Select(p => new WallEntryModel
            {
                Body = p.Body,
                CreatedBy = p.CreatedBy,
                EntryType = p.WallPostType.Id,
                Id = p.Id.ToString()
            });
        }

        public IEnumerable<WallEntryModel> GetEntries(IEnumerable<string> postIds)
        {
            //Retrieve from cache
            return wallEntryCacheRepo.GetByIds(postIds);
        }

        public void Dispose()
        {
            if (wallPostRepo != null)
            {
                wallPostRepo.Dispose();
            }

            if (wallEntryCacheRepo != null)
            {
                wallEntryCacheRepo.Dispose();
            }
        }

    }
}
