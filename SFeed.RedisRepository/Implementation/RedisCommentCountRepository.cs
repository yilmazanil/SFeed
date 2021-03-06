﻿using SFeed.Core.Infrastructure.Caching;
using SFeed.RedisRepository.Base;
using System;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisCommentCountRepository : RedisRepositoryBase, ICommentCountCacheRepository
    {
        public int GetCommentCount(string postId)
        {
            var entryKey = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            var value = db.StringGet(entryKey);
            return !string.IsNullOrWhiteSpace(value) ? Convert.ToInt32(value) : 0;
        }

        public void Remove(string postId)
        {
            var entryKey = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.KeyDelete(entryKey);
        }

        void ICommentCountCacheRepository.Decrement(string postId)
        {
            var entryKey = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.StringDecrement(entryKey);
        }

        void ICommentCountCacheRepository.Increment(string postId)
        {
            var entryKey = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.StringIncrement(entryKey);
        }
    }
}
