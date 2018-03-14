using System;
using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Comments;
using SFeed.RedisRepository.Base;
using System.Linq;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Caching;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisCommentRepository : RedisRepositoryBase, ICommentCacheRepository
    {
        public string RepoPrefix => RedisNameConstants.CommentRepoPrefix;
        public string ListRepoPrefix => RedisNameConstants.CommentLatestRepoPrefix;

        public int ListSize => RedisNameConstants.CommentRepoSize;

        public void AddComment(CommentCacheModel model)
        {
            var commentId = model.CommentId.ToString();
            var entryKey = GetEntryKey(RepoPrefix, commentId);
            var latestCommentsListKey = GetEntryKey(ListRepoPrefix, model.PostId);
            var postCommentCount = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, model.PostId);

            using (var redisClient = GetClientInstance())
            {
                //Save comment
                var clientApi = GetTypedClientApi<CommentCacheModel>(redisClient);
                clientApi.SetValue(entryKey, model);

                //Increment post comment count
                redisClient.Increment(postCommentCount, 1);

                //Update latest comments
                var list = redisClient.Lists[latestCommentsListKey];
                list.Prepend(commentId);

                if (list.Count > ListSize)
                {
                    list.Trim(1, ListSize);
                }
            }
        }
        public bool UpdateComment(CommentUpdateRequest model, DateTime modificationDate)
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

        public void RemoveComment(string postId, long commentId)
        {
            var entryKey = GetEntryKey(RepoPrefix, commentId.ToString());
            var listKey = GetEntryKey(ListRepoPrefix, postId);
            var postCommentCount = GetEntryKey(RedisNameConstants.CommentCounterNamePrefix, postId);

            using (var redisClient = GetClientInstance())
            {
                //remove comment
                redisClient.Remove(entryKey);
                //remove comment from latest comments list
                redisClient.Lists[listKey].Remove(commentId.ToString());
                //decrement total comment count
                var commentCount = redisClient.GetValue(postCommentCount);
                if (!string.IsNullOrWhiteSpace(commentCount) && Convert.ToInt32(commentCount) > 0)
                {
                    redisClient.Decrement(postCommentCount, 1);
                }
            }
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
        //public void RemoveAllComments(int maxRemovalSize)
        //{
        //    var searchPattern= GetEntrySearchPattern(RepoPrefix);
        //    var searchPatternLists = GetEntrySearchPattern(ListRepoPrefix);
        //    using (var redisClient = GetClientInstance())
        //    {
        //        var commentKeys = redisClient.ScanAllKeys(searchPattern, maxRemovalSize);
        //        var postListKeys = redisClient.ScanAllKeys(searchPattern, maxRemovalSize);
        //        redisClient.RemoveAll(commentKeys.Union(postListKeys));
        //    }
        //}
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

    }
}
