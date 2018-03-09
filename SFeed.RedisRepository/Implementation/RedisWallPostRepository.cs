using System.Collections.Generic;
using SFeed.Core.Infrastructure.Repository;
using SFeed.Core.Models.Newsfeed;
using SFeed.RedisRepository.Base;
using System.Linq;
using SFeed.Core.Models.WallPost;
using System;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisWallPostRepository : RedisRepositoryBase, IWallPostCacheRepository
    {
        public string CacheEntryPrefix => RedisNameConstants.WallPostRepoPrefix;

        public void AddItem(WallPostCacheModel model)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix, model.Id);
            using (var redisClient = GetClientInstance())
            {
                var clientApi = GetTypedClientApi<WallPostCacheModel>(redisClient);
                clientApi.SetValue(entryKey, model);
            }
        }

        public WallPostCacheModel GetItem(string postId)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix, postId);
            using (var redisClient = GetClientInstance())
            {
                var clientApi = GetTypedClientApi<WallPostCacheModel>(redisClient);
                return clientApi.GetValue(entryKey);
            }
        }

        public void UpdateItem(WallPostUpdateRequest model, DateTime modificationDate)
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

            var entryKeys = postIds.Select(t => GetEntryKey(CacheEntryPrefix,t));
            using (var redisClient = GetClientInstance())
            {
                return redisClient.GetAll<WallPostCacheModel>(entryKeys).Values;
            }
        }

        public void RemoveAll(int maxRemovalSize = 1000)
        {
            var searchPattern = GetEntrySearchPattern(CacheEntryPrefix);
            using (var redisClient = GetClientInstance())
            {
                var keys = redisClient.ScanAllKeys(searchPattern, maxRemovalSize);
                redisClient.RemoveAll(keys);
            }
        }

        public void RemoveItem(string postId)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix,postId);
            using (var redisClient = GetClientInstance())
            {
                redisClient.Remove(entryKey);
            }
        }

        public void RemoveItems(IEnumerable<string> postIds)
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
