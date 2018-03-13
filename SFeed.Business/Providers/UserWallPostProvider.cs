using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Models.WallPost;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Infrastructure.Repository;
using SFeed.RedisRepository.Implementation;
using SFeed.SqlRepository.Implementation;
using System;
using SFeed.Core.Infrastructure.Caching;
using SFeed.Core.Models.Wall;

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
            var result = wallPostRepo.SavePost(request);

            if (result != null && !string.IsNullOrWhiteSpace(result.PostId))
            {
                var cacheEntry = MapRequestToCacheModel(request, result);

                wallPostCacheRepo.SavePost(cacheEntry);

                return result.PostId;
            }
            return null;
        }

        public void UpdatePost(WallPostUpdateRequest model)
        {
            var modificationDate = wallPostRepo.UpdatePost(model);
            if (modificationDate.HasValue)
            {
                wallPostCacheRepo.UpdatePost(model, modificationDate.Value);
            }
        }

        public void DeletePost(string postId)
        {
            var deleted = wallPostRepo.RemovePost(postId);
            if (deleted)
            {
                wallPostCacheRepo.RemovePost(postId);
            }
        }

        public WallPostModel GetPost(string postId)
        {
            return wallPostRepo.GetPost(postId);
        }

        public IEnumerable<WallPostModel> GetUserWall(string userId, DateTime olderThan, int size)
        {
            return wallPostRepo.GetUserWall(userId, olderThan, size);
        }

        public IEnumerable<WallPostModel> GetGroupWall(string groupId, DateTime olderThan, int size)
        {
            return wallPostRepo.GetGroupWall(groupId, olderThan, size);
        }
        private WallPostCacheModel MapRequestToCacheModel(WallPostCreateRequest request, WallPostCreateResponse response)
        {
            return new WallPostCacheModel
            {
                Id = response.PostId,
                Body = request.Body,
                PostedBy = request.PostedBy,
                PostType = (short)request.PostType,
                TargetWall = new WallCacheModel { Id = request.TargetWall.OwnerId, WallOwnerTypeId = (short)request.TargetWall.WallOwnerType },
                CreatedDate = response.CreatedDate
            };
        }

    }
}
