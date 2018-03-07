using SFeed.Core.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using SFeed.Core.Infrastructue.Repository;
using SFeed.SqlRepository;
using AutoMapper;
using SFeed.Core.Models.WallPost;
using SFeed.Core.Models;
using SFeed.Core.Models.Newsfeed;

namespace SFeed.Business.Providers
{
    public class UserWallPostProvider : IUserWallPostProvider
    {
        private IRepository<WallPost> wallPostRepo;

        public UserWallPostProvider() : this(
            new WallPostRepository())
        {

        }

        public UserWallPostProvider(
            IRepository<WallPost> wallPostRepo)
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
                UserWall = new UserWall { UserId = request.WallOwner.Id, WallPostId = newPostId }
            };

            //Save Post
            wallPostRepo.Add(dbEntry);
            wallPostRepo.CommitChanges();


            return newPostId;
        }

        private WallPostCacheModel MapDbEntry(WallPost dbEntry)
        {
            return new WallPostCacheModel
            {
                Body = dbEntry.Body,
                Id = dbEntry.Id,
                PostedBy = dbEntry.CreatedBy,
                WallOwner = new Actor { Id = dbEntry.UserWall.UserId, ActorTypeId = (short)ActorType.user }
            };
        }
        public void UpdatePost(WallPostModel model)
        {
            var existingEntry = wallPostRepo.Get(p => p.Id == model.Id);
            existingEntry.Body = model.Body;
            existingEntry.ModifiedDate = DateTime.Now;
            existingEntry.PostType = Convert.ToByte(model.PostType);

            //Save Post
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
                result.PostedBy = new Actor { Id = dbEntry.CreatedBy, ActorTypeId = (short)ActorType.user };
                return result;
            }
            return null;
        }

        public IEnumerable<WallPostModel> GetUserWall(string wallOwnerId)
        {
            return wallPostRepo.GetMany(p => p.UserWall.UserId == wallOwnerId && p.IsDeleted == false).Select(p => new WallPostModel
            {
                Body = p.Body,
                PostedBy = new Actor { Id = p.CreatedBy, ActorTypeId = (short)ActorType.user }, 
                PostType = p.PostType,
                Id = p.Id.ToString()
            });
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (wallPostRepo != null)
                {
                    wallPostRepo.Dispose();
                }

            }
        }
    }
}
