//using SFeed.Core.Infrastructure.Caching;
//using SFeed.RedisRepository.Base;
//using System;

//namespace SFeed.RedisRepository.Implementation
//{
//    public class RedisCommentCountRepository : RedisRepositoryBase, ICommentCountCacheRepository
//    {
//        public int GetCommentCount(string postId)
//        {
//            var entryKey = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);
//            using (var client = GetClientInstance())
//            {
//                var value = client.GetValue(entryKey);
//                return !string.IsNullOrWhiteSpace(value) ? Convert.ToInt32(value) : 0;
//            }
//        }

//        public void Remove(string postId)
//        {
//            var entryKey = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);
//            using (var client = GetClientInstance())
//            {
//                client.Remove(entryKey);
//            }
//        }

//        void ICommentCountCacheRepository.Decrement(string postId)
//        {
//            var entryKey = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);
//            Decrement(entryKey);
//        }

//        void ICommentCountCacheRepository.Increment(string postId)
//        {
//            var entryKey = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);
//            Increment(entryKey);
//        }
//    }
//}
