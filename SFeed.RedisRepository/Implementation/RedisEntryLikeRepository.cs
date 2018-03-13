using SFeed.Core.Infrastructure.Caching;
using SFeed.RedisRepository.Base;
using System;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisEntryLikeRepository : RedisRepositoryBase, IEntryLikeCacheRepository
    {
        public string postLikeCounterPrefix => RedisNameConstants.PostLikeCounterNamePrefix;
        public string commentLikeCounterPrefix => RedisNameConstants.CommentLikeCounterNamePrefix;

        public void DecrementCommentLikeCount(long commentId)
        {
            var entryKey = GetEntryKey(commentLikeCounterPrefix, commentId.ToString());
            Decrement(entryKey);
        }

        public void DecrementPostLikeCount(string postId)
        {
            var entryKey = GetEntryKey(postLikeCounterPrefix, postId);
            Decrement(entryKey);
        }

        public void IncrementCommentLikeCount(long commentId)
        {
            var entryKey = GetEntryKey(commentLikeCounterPrefix , commentId.ToString());
            Increment(entryKey);
        }

        public void IncrementPostLikeCount(string postId)
        {
            var entryKey = GetEntryKey(postLikeCounterPrefix, postId);
            Increment(entryKey);
        }

        private void Increment(string key)
        {
            using (var client = GetClientInstance())
            {
                client.Increment(key, 1);
            }
        }

        private void Decrement(string key)
        {
            using (var client = GetClientInstance())
            {
                var value = client.GetValue(key);
                if (!string.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > 0)
                {
                    client.Decrement(key, 1);
                }
            }
        }

        public int GetPostLikeCount(string postId)
        {
            var entryKey = GetEntryKey(postLikeCounterPrefix, postId);
            using (var client = GetClientInstance())
            {
                var value = client.GetValue(entryKey);
                return !string.IsNullOrWhiteSpace(value) ? Convert.ToInt32(value) : 0;
            }
        }

        public int GetCommentLikeCount(long commentId)
        {
            var entryKey = GetEntryKey(commentLikeCounterPrefix, commentId.ToString());
            using (var client = GetClientInstance())
            {
                var value = client.GetValue(entryKey);
                return !string.IsNullOrWhiteSpace(value) ? Convert.ToInt32(value) : 0;
            }
        }
    }
}
