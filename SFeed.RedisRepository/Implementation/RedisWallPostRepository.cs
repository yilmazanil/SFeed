using System.Collections.Generic;
using SFeed.Core.Models.Newsfeed;
using SFeed.RedisRepository.Base;
using System.Linq;
using SFeed.Core.Models.WallPost;
using System;
using SFeed.Core.Infrastructure.Caching;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisWallPostRepository : RedisRepositoryBase, IWallPostCacheRepository
    {
        public string CacheEntryPrefix => RedisNameConstants.WallPostRepoPrefix;

        public void SavePost(WallPostCacheModel model)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix, model.Id);
            using (var redisClient = GetClientInstance())
            {
                var clientApi = GetTypedClientApi<WallPostCacheModel>(redisClient);
                clientApi.SetValue(entryKey, model);
            }
        }

        public WallPostCacheModel GetPost(string postId)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix, postId);
            using (var redisClient = GetClientInstance())
            {
                var clientApi = GetTypedClientApi<WallPostCacheModel>(redisClient);
                return clientApi.GetValue(entryKey);
            }
        }

        public void UpdatePost(WallPostUpdateRequest model, DateTime modificationDate)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix, model.PostId);
            using (var redisClient = GetClientInstance())
            {
                var clientApi = GetTypedClientApi<WallPostCacheModel>(redisClient);
                var existingItem = clientApi.GetValue(entryKey);
                if (existingItem != null)
                {
                    existingItem.Body = model.Body;
                    existingItem.PostType = (short)model.PostType;
                    existingItem.ModifiedDate = modificationDate;
                    clientApi.SetValue(entryKey, existingItem);
                }
            }

        }

        public IEnumerable<WallPostCacheModel> GetItems(IEnumerable<string> postIds)
        {
            var entryKeys = postIds.Select(t => GetEntryKey(CacheEntryPrefix, t));
            using (var redisClient = GetClientInstance())
            {
                return redisClient.GetAll<WallPostCacheModel>(entryKeys).Values;
            }
        }

        //public void RemoveAllPosts(int maxRemovalSize = 1000)
        //{
        //    var searchPattern = GetEntrySearchPattern(CacheEntryPrefix);
        //    using (var redisClient = GetClientInstance())
        //    {
        //        var keys = redisClient.ScanAllKeys(searchPattern, maxRemovalSize);
        //        redisClient.RemoveAll(keys);
        //    }
        //}

        public void RemovePost(string postId)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix, postId);
            using (var redisClient = GetClientInstance())
            {
                redisClient.Remove(entryKey);
            }
        }

        public void RemovePosts(IEnumerable<string> postIds)
        {
            var keyList = new List<string>();
            foreach (var postId in postIds)
            {
                keyList.Add(GetEntryKey(CacheEntryPrefix, postId));
            }

            using (var redisClient = GetClientInstance())
            {
                redisClient.RemoveAll(keyList);
            }
        }

    }
}
