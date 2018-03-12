using System;
using SFeed.Core.Infrastructure.Repository.Caching;
using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Comments;
using SFeed.RedisRepository.Base;
using System.Linq;
using System.Collections.Generic;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisCommentRepository : RedisRepositoryBase, ICommentCacheRepository
    {
        public string RepoPrefix => RedisNameConstants.CommentRepoPrefix;
        public string ListRepoPrefix => RedisNameConstants.CommentLatestRepoPrefix;

        public int ListSize => RedisNameConstants.CommentRepoSize;

        public void AddItem(CommentCacheModel model)
        {
            var entryKey = GetEntryKey(RepoPrefix, model.CommentId.ToString());
            var listKey = GetEntryKey(ListRepoPrefix, model.PostId);
            var postCommentCount = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, model.PostId);

            using (var redisClient = GetClientInstance())
            {
                var clientApi = GetTypedClientApi<CommentCacheModel>(redisClient);
                clientApi.SetValue(entryKey, model);
                redisClient.Increment(postCommentCount, 1);
                var list = redisClient.Lists[listKey];
                if (list.Count >= ListSize)
                {
                    list.Trim(0, ListSize - 1);
                }
                list.Append(model.CommentId.ToString());


            }
        }

        public void RemoveAll(int maxRemovalSize)
        {
            var searchPattern= GetEntrySearchPattern(RepoPrefix);
            var searchPatternLists = GetEntrySearchPattern(ListRepoPrefix);
            using (var redisClient = GetClientInstance())
            {
                var commentKeys = redisClient.ScanAllKeys(searchPattern, maxRemovalSize);
                var postListKeys = redisClient.ScanAllKeys(searchPattern, maxRemovalSize);
                redisClient.RemoveAll(commentKeys.Union(postListKeys));
            }
        }

        public void RemoveComment(string postId, long commentId)
        {
            var entryKey = GetEntryKey(RepoPrefix, commentId.ToString());
            var listKey = GetEntryKey(ListRepoPrefix, postId);
            var postCommentCount = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);

            using (var redisClient = GetClientInstance())
            {
                redisClient.Remove(entryKey);
                redisClient.Lists[listKey].Remove(commentId.ToString());

                var commentCount = redisClient.GetValue(postCommentCount);
                if (!string.IsNullOrWhiteSpace(commentCount) && Convert.ToInt32(commentCount) > 0)
                {
                    redisClient.Decrement(postCommentCount, 1);
                }
            }
        }

        public bool UpdateItem(CommentUpdateRequest model, DateTime modificationDate)
        {
            var entryKey = GetEntryKey(RepoPrefix, model.CommentId.ToString());

            using (var redisClient = GetClientInstance())
            {

                var clientApi = GetTypedClientApi<CommentCacheModel>(redisClient);
                var existingItem = clientApi.GetValue(entryKey);
                if (existingItem != null)
                {
                    existingItem.ModifiedDate = modificationDate;
                    existingItem.Body = model.Body;
                    clientApi.SetValue(entryKey, existingItem);
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<CommentCacheModel> GetLatestComments(string postId)
        {
            var result = new List<CommentCacheModel>();

            var listKey = GetEntryKey(ListRepoPrefix, postId);

            using (var client = GetClientInstance())
            {
                var comments = client.Lists[listKey].GetAll();
                if (comments != null && comments.Any())
                {
                    var clientApi = GetTypedClientApi<CommentCacheModel>(client);

                    foreach (var comment in comments)
                    {
                        var repoPrefix = GetEntryKey(RepoPrefix, comment);
                        result.Add(clientApi.GetValue(repoPrefix));
                    }
                }
            }
            return result;

        }

        public int GetCommentCount(string postId)
        {
            var entryKey = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);
            using (var client = GetClientInstance())
            {
                var value = client.GetValue(entryKey);
                return !string.IsNullOrWhiteSpace(value) ? Convert.ToInt32(value) : 0;
            }
        }

        private void IncrementCommentCount(string postId)
        {
            var entryKey = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);
            Increment(entryKey);
        }

        private void DecrementCommentCount(string postId)
        {
            var entryKey = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);
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
    }
}
