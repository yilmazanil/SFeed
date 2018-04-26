using SFeed.Core.Infrastructure.Caching;
using SFeed.RedisRepository.Base;
using System.Collections.Generic;

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
                redisClient.AddItemToSet(entryKey,followerId);
            }
        }

        public void FollowGroup(string groupId, string followerId)
        {
            var entryKey = GetEntryKey(GroupCachePrefix, groupId);
            using (var redisClient = GetClientInstance())
            {
                redisClient.AddItemToSet(entryKey, followerId);
            }
        }

        public void UnfollowUser(string userId, string followerId)
        {
            var entryKey = GetEntryKey(UserCachePrefix, userId);
            using (var redisClient = GetClientInstance())
            {
                redisClient.RemoveItemFromSet(entryKey, followerId);
            }
        }

        public void UnfollowGroup(string groupId, string followerId)
        {
            var entryKey = GetEntryKey(GroupCachePrefix, groupId);
            using (var redisClient = GetClientInstance())
            {
                redisClient.RemoveItemFromSet(entryKey, followerId);
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

        public void ResetUserFollowers(string userId, IEnumerable<string> followerIds)
        {
            var entryKey = GetEntryKey(UserCachePrefix, userId);
            using (var redisClient = GetClientInstance())
            {
                using (var transaction = redisClient.CreateTransaction())
                {
                    transaction.QueueCommand(t => t.RemoveEntry(entryKey));

                    foreach (var followerId in followerIds)
                    {
                        transaction.QueueCommand(t => t.AddItemToSet(entryKey, followerId));
                    }
                    transaction.Commit();
                }
            }
        }

        public void ResetGroupFollowers(string groupId, IEnumerable<string> followerIds)
        {
            var entryKey = GetEntryKey(GroupCachePrefix, groupId);
            using (var redisClient = GetClientInstance())
            {
                using (var transaction = redisClient.CreateTransaction())
                {
                    transaction.QueueCommand(t => t.RemoveEntry(entryKey));

                    foreach (var followerId in followerIds)
                    {
                        transaction.QueueCommand(t => t.AddItemToSet(entryKey, followerId));
                    }
                    transaction.Commit();
                }
            }
        }
    }
}
