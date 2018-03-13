using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.SqlRepository.Implementation;
using SFeed.RedisRepository.Implementation;
using SFeed.Core.Infrastructure.Caching;
using SFeed.Core.Infrastructure.Repository;
using SFeed.Core.Models.Follower;
using log4net;
using System;

namespace SFeed.Business.Providers
{
    public class FollowerProvider : IFollowerProvider
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(IFollowerProvider));

        IFollowerCacheRepository followerCacheRepo;
        IFollowerRepository followerRepo;

        public FollowerProvider() : this(
            new RedisFollowerRepository(),
            new FollowerRepository()
            )
        {

        }

        public FollowerProvider(
            IFollowerCacheRepository followerCacheRepo,
            IFollowerRepository followerRepo)
        {
            this.followerCacheRepo = followerCacheRepo;
            this.followerRepo = followerRepo;
        }

        public void FollowUser(string followerId, string userId)
        {
            followerRepo.FollowUser(userId, followerId);
            try
            {
                followerCacheRepo.FollowUser(userId, followerId);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format(
                    "[FollowUser] Error updating follower cache for parameters followerId:{0}, userId:{1}", followerId, userId),ex);
            }
        }

        public void UnfollowUser(string followerId, string userId)
        {
            followerRepo.UnfollowUser(userId, followerId);
            try
            {
                followerCacheRepo.UnfollowUser(userId, followerId);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format(
                    "[UnfollowUser] Error updating follower cache for parameters followerId:{0}, userId:{1}", followerId, userId), ex);
            }
        }

        public void FollowGroup(string followerId, string groupId)
        {
            followerRepo.FollowGroup(groupId, followerId);
            try
            {
                followerCacheRepo.FollowGroup(groupId, followerId);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format(
                    "[FollowGroup] Error updating follower cache for parameters followerId:{0}, groupId:{1}", followerId, groupId), ex);
            }
        }

        public void UnfollowGroup(string followerId, string groupId)
        {
            followerRepo.UnfollowGroup(groupId, followerId);
            try
            {
                followerCacheRepo.UnfollowGroup(groupId, followerId);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format(
                    "[UnfollowGroup] Error updating follower cache for parameters followerId:{0}, groupId:{1}", followerId, groupId), ex);
            }
        }

        public IEnumerable<string> GetUserFollowers(string userId)
        {
            return followerRepo.GetFollowersUser(userId);
        }

        public IEnumerable<string> GetGroupFollowers(string groupId)
        {
            return followerRepo.GetFollowersGroup(groupId);
        }

        public FollowerPagedModel GetUserFollowersPaged(string userId, int skip, int size)
        {
            return followerRepo.GetFollowersUserPaged(userId, skip, size);
        }

        public FollowerPagedModel GetGroupFollowersPaged(string groupId, int skip, int size)
        {
            return followerRepo.GetFollowersGroupPaged(groupId, skip, size);
        }

        public IEnumerable<string> GetUserFollowersCached(string userId)
        {
            return followerCacheRepo.GetUserFollowers(userId);
        }

        public IEnumerable<string> GetGroupFollowersCached(string groupId)
        {
            return followerCacheRepo.GetGroupFollowers(groupId);
        }

        public FollowerPagedModel GetFollowingUsersPaged(string userId, int skip, int size)
        {
            return followerRepo.GetFollowingUsersPaged(userId, skip, size);
        }

        public FollowerPagedModel GetFollowingGroupsPaged(string groupId, int skip, int size)
        {
            return followerRepo.GetFollowingGroupsPaged(groupId, skip, size);
        }
    }
}
