using System;
using SFeed.Core.Infrastructure.Repository.Caching;
using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Comments;
using SFeed.RedisRepository.Base;
using System.Linq;

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
            using (var redisClient = GetClientInstance())
            {
                var clientApi = GetTypedClientApi<CommentCacheModel>(redisClient);
                clientApi.SetValue(entryKey, model);

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
            using (var redisClient = GetClientInstance())
            {
                redisClient.Remove(entryKey);
                redisClient.Lists[listKey].Remove(commentId.ToString());
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
    }
}
