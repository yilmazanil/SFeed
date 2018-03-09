using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Models.WallPost;
using SFeed.Core.Models;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Infrastructure.Repository;
using SFeed.RedisRepository.Implementation;
using SFeed.Core.Infrastructure.Repository.Sql;
using SFeed.SqlRepository.Implementation;
using System;

namespace SFeed.Business.Providers
{
    public class UserWallPostProvider : IWallPostProvider
    {
        private IWallPostRepository wallPostRepo;
        private IWallPostCacheRepository wallPostCacheRepo;

        public UserWallPostProvider() : this(
            new WallPostRepository(),
            new RedisWallPostRepository())
        {

        }

        public UserWallPostProvider(
            IWallPostRepository wallPostRepo,
            IWallPostCacheRepository wallPostCacheRepo)
        {
            this.wallPostRepo = wallPostRepo;
            this.wallPostCacheRepo = wallPostCacheRepo;
        }

        public string AddPost(WallPostCreateRequest request)
        {
            var result = wallPostRepo.SaveItem(request);

            if (result != null && !string.IsNullOrWhiteSpace(result.PostId))
            {
                var cacheEntry = MapRequestToCacheModel(request, result);

                wallPostCacheRepo.AddItem(cacheEntry);

                return result.PostId;
            }
            return null;
        }

        public void UpdatePost(WallPostUpdateRequest model)
        {
            var modificationDate = wallPostRepo.UpdateItem(model);
            if (modificationDate.HasValue)
            {
                wallPostCacheRepo.UpdateItem(model, modificationDate.Value);
            }
        }

        public void DeletePost(string postId)
        {
            var deleted = wallPostRepo.RemoveItem(postId);
            if (deleted)
            {
                wallPostCacheRepo.RemoveItem(postId);
            }
        }

        public WallPostModel GetPost(string postId)
        {
            return wallPostRepo.GetItem(postId);
        }

        public IEnumerable<WallPostModel> GetUserWall(WallOwner wallOwner, DateTime olderThan, int size)
        {
            return wallPostRepo.GetUserWall(wallOwner, olderThan, size);
        }

        private WallPostCacheModel MapRequestToCacheModel(WallPostCreateRequest request, WallPostCreateResponse response)
        {
            return new WallPostCacheModel
            {
                Id = response.PostId,
                Body = request.Body,
                PostedBy = request.PostedBy,
                PostType = (short)request.PostType,
                WallOwner = new WallOwnerCacheModel { Id = request.WallOwner.Id, WallOwnerTypeId = (short)request.WallOwner.WallOwnerType },
                CreatedDate = response.CreatedDate
            };
        }


    }
}
