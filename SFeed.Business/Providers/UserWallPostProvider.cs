using SFeed.Core.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using SFeed.Core.Infrastructue.Repository;
using SFeed.SqlRepository;
using AutoMapper;
using SFeed.Core.Models.WallPost;

namespace SFeed.Business.Providers
{
    public class UserWallPostProvider : IUserWallPostProvider
    {
        private IRepository<WallPost> wallPostRepo;

        public UserWallPostProvider() : this(
            new WallPostRepository())
        {

        }

        public UserWallPostProvider(IRepository<WallPost> wallPostRepo)
        {
            this.wallPostRepo = wallPostRepo;
        }

        public string AddPost(WallPostCreateRequest request)
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

            return newPostId;

        }

        public void UpdatePost(WallPostModel model)
        {
            var existingEntry = wallPostRepo.Get(p => p.Id == model.Id);
            existingEntry.Body = model.Body;
            existingEntry.ModifiedDate = DateTime.Now;
            existingEntry.PostType = Convert.ToByte(model.PostType);

            //Update DB
            wallPostRepo.Update(existingEntry);
            wallPostRepo.CommitChanges();
        }

        public void DeletePost(string postId)
        {
            //Mark as deleted
            wallPostRepo.Delete(p => p.Id == postId);
            wallPostRepo.CommitChanges();

        }
        public WallPostModel GetPost(string postId)
        {
            var dbEntry =  wallPostRepo.Get(e => e.Id == postId && e.IsDeleted == false);
            if (dbEntry != null)
            {
                var result = Mapper.Map<WallPostModel>(dbEntry);
                result.PostedBy = dbEntry.CreatedBy;
                return result;
            }
            return null;
        }

        public IEnumerable<WallPostModel> GetUserWall(string wallOwnerId)
        {
            return wallPostRepo.GetMany(p => p.UserWall.UserId == wallOwnerId && p.IsDeleted == false).Select(p => new WallPostModel
            {
                Body = p.Body,
                PostedBy = p.CreatedBy,
                PostType = p.PostType,
                Id = p.Id.ToString()
            });
        }


        public void Dispose()
        {
            if (wallPostRepo != null)
            {
                wallPostRepo.Dispose();
            }
        }

    }
}
