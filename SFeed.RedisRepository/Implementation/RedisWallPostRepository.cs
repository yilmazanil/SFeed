using System.Collections.Generic;
using SFeed.Core.Models.Newsfeed;
using SFeed.RedisRepository.Base;
using System.Linq;
using SFeed.Core.Models.WallPost;
using System;
using SFeed.Core.Infrastructure.Caching;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisWallPostRepository : RedisRepositoryBase, IWallPostCacheRepository
    {
        public string CacheEntryPrefix => RedisNameConstants.WallPostRepoPrefix;

        public void SavePost(WallPostCacheModel model)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix, model.Id);
            var item = JsonConvert.SerializeObject(model);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.StringSet(entryKey, item);
        }

        public WallPostCacheModel GetPost(string postId)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix, postId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            var item = db.StringGet(entryKey);
            return !string.IsNullOrWhiteSpace(item) ? JsonConvert.DeserializeObject<WallPostCacheModel>(item) : null;
        }

        public void UpdatePost(WallPostUpdateRequest model, DateTime modificationDate)
        {
            var post = GetPost(model.PostId);
            if (post != null)
            {
                post.Body = model.Body;
                post.PostType = (short)model.PostType;
                post.ModifiedDate = modificationDate;
                SavePost(post);
            }

        }

        public void RemovePost(string postId)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix, postId);
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.KeyDelete(entryKey);
        }

        public IEnumerable<WallPostCacheModel> GetItems(IEnumerable<string> postIds)
        {
            var result = new List<WallPostCacheModel>();
            var entryKeys = postIds.Select(t => GetEntryKey(CacheEntryPrefix, t)).Select(p => (RedisKey)p).ToArray();
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            var results = db.StringGet(entryKeys);

            foreach (var resultItem in results)
            {
                if (!string.IsNullOrWhiteSpace(resultItem))
                {
                    result.Add(JsonConvert.DeserializeObject<WallPostCacheModel>(resultItem));
                }
            }

            return result;
        }

        public void RemovePosts(IEnumerable<string> postIds)
        {
            var keyList = new List<RedisKey>();
            foreach (var postId in postIds)
            {
                keyList.Add(GetEntryKey(CacheEntryPrefix, postId));
            }
            var db = StackExchangeRedisConnectionProvider.GetDataBase();
            db.KeyDelete(keyList.ToArray());
        }

    }
}
