using System;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Repository.Caching;
using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Comments;
using SFeed.RedisRepository.Base;
using System.Linq;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisCommentRepository : RedisRepositoryBase, ICommentCacheRepository
    {
        public string CacheEntryPrefix => RedisNameConstants.CommentRepoPrefix;

        public int ListSize => RedisNameConstants.CommentRepoSize;

        public void AddItem(CommentCacheModel model)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix, model.PostId);
            using (var redisClient = GetClientInstance())
            {
                var clientApi = GetTypedClientApi<CommentCacheModel>(redisClient);
                var list = clientApi.Lists[entryKey];
                if (list.Count >= ListSize)
                {
                    list.Trim(1, ListSize);
                }
                list.Prepend(model);
            }
        }

        public void RemoveAll(int maxRemovalSize)
        {
            var searchPattern = GetEntrySearchPattern(CacheEntryPrefix);
            using (var redisClient = GetClientInstance())
            {
                var keys = redisClient.ScanAllKeys(searchPattern, maxRemovalSize);
                redisClient.RemoveAll(keys);
            }
        }

        public void RemoveComment(string postId, long commentId)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix, postId);
            using (var redisClient = GetClientInstance())
            {
                var clientApi = GetTypedClientApi<CommentCacheModel>(redisClient);
                var listRef = clientApi.Lists[entryKey];
                var existingComment =
                    listRef.FirstOrDefault(t => t.CommentId == commentId);
                if (existingComment != null)
                {
                    clientApi.RemoveItemFromList(listRef, existingComment);
                }
            }
        }

        public void RemoveComments(string postId)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix, postId);
            using (var redisClient = GetClientInstance())
            {
                redisClient.Remove(entryKey);
            }
        }

        public void RemoveComments(IEnumerable<string> postIds)
        {
            var entryList = new List<string>();
            foreach (var postId in postIds)
            {
                entryList.Add(GetEntryKey(CacheEntryPrefix, postId));
            }
           
            using (var redisClient = GetClientInstance())
            {
                redisClient.RemoveAll(entryList);
            }
        }

        public void UpdateItem(CommentUpdateRequest model, DateTime modificationDate)
        {
            var entryKey = GetEntryKey(CacheEntryPrefix, model.PostId);
            using (var redisClient = GetClientInstance())
            {
                var clientApi = GetTypedClientApi<CommentCacheModel>(redisClient);
                var listRef = clientApi.Lists[entryKey];
                var existingComment = listRef.FirstOrDefault(t => t.CommentId == model.CommentId);
                if (existingComment != null)
                {
                    listRef.Remove(existingComment);

                    var doubleCheckIndex = listRef.IndexOf(existingComment);
                    if(doubleCheckIndex )
                    existingComment.Body = model.Body;
                    existingComment.ModifiedDate = modificationDate;

                   
                    if(listRef.Contains()
                    redisClient.SetItemInList()
                }
            }
        }
    }
}
