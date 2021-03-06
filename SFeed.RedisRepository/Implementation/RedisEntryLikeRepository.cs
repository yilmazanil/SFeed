﻿using SFeed.Core.Infrastructure.Caching;
using SFeed.RedisRepository.Base;
using System;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisEntryLikeRepository : RedisRepositoryBase, ILikeCountCacheRepository
    {
        public string postLikeCounterPrefix => RedisNameConstants.PostLikeCounterNamePrefix;
        public string commentLikeCounterPrefix => RedisNameConstants.CommentLikeCounterNamePrefix;

        public void DecrementCommentLikeCount(long commentId)
        {
            var entryKey = GetEntryKey(commentLikeCounterPrefix, commentId.ToString());
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.StringDecrement(entryKey);
        }

        public void DecrementPostLikeCount(string postId)
        {
            var entryKey = GetEntryKey(postLikeCounterPrefix, postId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.StringDecrement(entryKey);
        }

        public void IncrementCommentLikeCount(long commentId)
        {
            var entryKey = GetEntryKey(commentLikeCounterPrefix , commentId.ToString());
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.StringIncrement(entryKey);
        }

        public void IncrementPostLikeCount(string postId)
        {
            var entryKey = GetEntryKey(postLikeCounterPrefix, postId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.StringIncrement(entryKey);
        }
        public int GetPostLikeCount(string postId)
        {
            var entryKey = GetEntryKey(postLikeCounterPrefix, postId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            var value = db.StringGet(entryKey);
            return !string.IsNullOrWhiteSpace(value) ? Convert.ToInt32(value) : 0;
        }

        public int GetCommentLikeCount(long commentId)
        {
            var entryKey = GetEntryKey(commentLikeCounterPrefix, commentId.ToString());
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            var value = db.StringGet(entryKey);
            return !string.IsNullOrWhiteSpace(value) ? Convert.ToInt32(value) : 0;
        }
    }
}
