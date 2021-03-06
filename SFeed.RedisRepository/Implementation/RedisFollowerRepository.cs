﻿using SFeed.Core.Infrastructure.Caching;
using SFeed.RedisRepository.Base;
using System.Collections.Generic;
using System.Linq;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisFollowerRepository : RedisRepositoryBase, IFollowerCacheRepository
    {
        public string UserCachePrefix => RedisNameConstants.UserFollowerRepoPrefix;
        public string GroupCachePrefix => RedisNameConstants.GroupFollowerRepoPrefix;

        public void FollowUser(string userId, string followerId)
        {
            var entryKey = GetEntryKey(UserCachePrefix, userId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.SetAdd(entryKey, followerId);
        }

        public void FollowGroup(string groupId, string followerId)
        {
            var entryKey = GetEntryKey(GroupCachePrefix, groupId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.SetAdd(entryKey, followerId);
        }

        public void UnfollowUser(string userId, string followerId)
        {
            var entryKey = GetEntryKey(UserCachePrefix, userId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.SetRemove(entryKey, followerId);
        }

        public void UnfollowGroup(string groupId, string followerId)
        {
            var entryKey = GetEntryKey(GroupCachePrefix, groupId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.SetRemove(entryKey, followerId);
        }

        public IEnumerable<string> GetUserFollowers(string userId)
        {
            var entryKey = GetEntryKey(UserCachePrefix, userId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            return db.SetMembers(entryKey).Select(p=>p.ToString());
        }

        public IEnumerable<string> GetGroupFollowers(string groupId)
        {
            var entryKey = GetEntryKey(GroupCachePrefix, groupId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            return db.SetMembers(entryKey).Select(p => p.ToString());
        }
    }
}
