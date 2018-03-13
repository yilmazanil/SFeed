using SFeed.Core.Infrastructure.Caching;
using SFeed.RedisRepository.Base;
using System.Collections.Generic;
using SFeed.Core.Models.Follower;
using System;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisFollowerRepository : RedisRepositoryBase, IFollowerCacheRepository
    {
        public string UserCachePrefix => RedisNameConstants.UserFollowerRepoPrefix;
        public string GroupCachePrefix => RedisNameConstants.GroupFollowerRepoPrefix;

        public void FollowUser(string userId, string followerId)
        {
            var entryKey = GetEntryKey(UserCachePrefix, userId);
            using (var redisClient = GetClientInstance())
            {
                redisClient.Sets[entryKey].Add(followerId);
            }
        }

        public void FollowGroup(string groupId, string followerId)
        {
            var entryKey = GetEntryKey(GroupCachePrefix, groupId);
            using (var redisClient = GetClientInstance())
            {
                redisClient.Sets[entryKey].Add(followerId);
            }
        }

        public void UnfollowUser(string userId, string followerId)
        {
            var entryKey = GetEntryKey(UserCachePrefix, userId);
            using (var redisClient = GetClientInstance())
            {
                redisClient.Sets[entryKey].Remove(followerId);
            }
        }

        public void UnfollowGroup(string groupId, string followerId)
        {
            var entryKey = GetEntryKey(GroupCachePrefix, groupId);
            using (var redisClient = GetClientInstance())
            {
                redisClient.Sets[entryKey].Remove(followerId);
            }
        }

        public IEnumerable<string> GetUserFollowers(string userId)
        {
            var entryKey = GetEntryKey(UserCachePrefix, userId);
            using (var redisClient = GetClientInstance())
            {
                return redisClient.Sets[entryKey].GetAll();
            }
        }

        public IEnumerable<string> GetGroupFollowers(string groupId)
        {
            var entryKey = GetEntryKey(GroupCachePrefix, groupId);
            using (var redisClient = GetClientInstance())
            {
               return redisClient.Sets[entryKey].GetAll();
            }
        }
    }
}
