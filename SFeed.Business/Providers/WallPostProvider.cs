﻿using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Models.WallPost;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Infrastructure.Repository;
using SFeed.RedisRepository.Implementation;
using SFeed.SqlRepository.Implementation;
using System;
using SFeed.Core.Infrastructure.Caching;
using SFeed.Core.Models.Wall;
using log4net;
using SFeed.Core.Models.GroupWall;
using System.Linq;

namespace SFeed.Business.Providers
{
    public class WallPostProvider : IWallPostProvider
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(WallPostProvider));

        private IWallPostRepository wallPostRepo;
        private IWallPostCacheRepository wallPostCacheRepo;
        private IGroupWallRepository groupWallCacheRepo;

        public WallPostProvider() : this(
            new WallPostRepository(),
            new RedisWallPostRepository(),
            new RedisGroupWallRepository())
        {

        }

        public WallPostProvider(
            IWallPostRepository wallPostRepo,
            IWallPostCacheRepository wallPostCacheRepo,
            IGroupWallRepository groupWallCacheRepo)
        {
            this.wallPostRepo = wallPostRepo;
            this.wallPostCacheRepo = wallPostCacheRepo;
            this.groupWallCacheRepo = groupWallCacheRepo;
        }

        public string AddPost(WallPostCreateRequest request)
        {
            var result = wallPostRepo.SavePost(request);

            if (result != null && !string.IsNullOrWhiteSpace(result.PostId))
            {
                try
                {
                    var cacheEntry = MapRequestToCacheModel(request, result);
                    wallPostCacheRepo.SavePost(cacheEntry);
                    if (request.TargetWall.WallOwnerType == WallType.group)
                    {
                        var postIds = wallPostRepo.GetGroupWallIds(request.TargetWall.OwnerId, 40);
                        groupWallCacheRepo.RefreshGroupWall(request.TargetWall.OwnerId, postIds);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format(
                    "[AddPost] Error updating wallPost cache for postId : {0}", result), ex);
                }
                return result.PostId;
            }
            return null;
        }

        public void UpdatePost(WallPostUpdateRequest model)
        {
            var modificationDate = wallPostRepo.UpdatePost(model);
            if (modificationDate.HasValue)
            {
                try
                {
                    wallPostCacheRepo.UpdatePost(model, modificationDate.Value);
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format(
                        "[UpdatePost] Error updating wallPost cache for postId : {0}", model.PostId), ex);
                }
            }
        }

        public void DeletePost(string postId)
        {
            var deleted = wallPostRepo.RemovePost(postId);
            if (deleted)
            {
                try
                {
                    wallPostCacheRepo.RemovePost(postId);
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format(
                        "[DeletePost] Error updating wallPost cache for postId : {0}", postId), ex);
                }
            }
        }
        public WallPostModel GetPost(string postId)
        {
            return wallPostRepo.GetPost(postId);
        }

        public WallPostWithDetailsModel GetPostDetailed(string postId)
        {
            return wallPostRepo.GetPostDetailed(postId);
        }

        public IEnumerable<WallPostModel> GetUserWall(string userId, DateTime olderThan, int size)
        {
            return wallPostRepo.GetUserWall(userId, olderThan, size);
        }

        public IEnumerable<WallPostWithDetailsModel> GetUserWallDetailed(string userId, DateTime olderThan, int size)
        {
            return wallPostRepo.GetUserWallDetailed(userId, olderThan, size);
        }

        public IEnumerable<GroupWallResponseModel> GetGroupWall(string groupId, int skip, int size)
        {
            if (skip > 0 || size > 100)
            {
                return wallPostRepo.GetGroupWall(groupId, skip, size);
            }
            else
            {
                var cachedPosts = groupWallCacheRepo.GetGroupWall(groupId);
                if (cachedPosts != null && cachedPosts.Any())
                {
                    return cachedPosts;
                }
                else
                {
                    return wallPostRepo.GetGroupWall(groupId, skip, size);
                }
            }
        }

        public IEnumerable<WallPostWithDetailsModel> GetGroupWallDetailed(string groupId, DateTime olderThan, int size)
        {
            return wallPostRepo.GetGroupWallDetailed(groupId, olderThan, size);
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
