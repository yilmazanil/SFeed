using ServiceStack.Redis;
using SFeed.Core.Infrastructure.Repository;
using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.RedisRepository
{
    public class RedisNewsfeedResponseRepository : IReadonlyItemRepository<NewsfeedWallPostModel>
    {
        private IRedisClient client;


        protected IRedisClient Client
        {
            get
            {
                if (client == null)
                {
                    client = RedisConnectionProvider.GetClient();
                }
                return client;
            }
        }
        public NewsfeedWallPostModel GetItem(string id)
        {
            var wallPost = Client.As<WallPostCacheModel>().GetValue(string.Concat(RedisNameConstants.WallPostRepoPrefix, ":", id));
            var responseModel = new NewsfeedWallPostModel();

            if (wallPost != null)
            {
                responseModel.Body = wallPost.Body;
                responseModel.Id = wallPost.Id;
                responseModel.PostedBy = wallPost.PostedBy;
                responseModel.PostType = wallPost.PostType;
                var commentsKey = string.Concat(RedisNameConstants.CommentCounterNamePrefix, ":", wallPost.Id);
                var comments = Client.As<CommentCacheModel>().Lists[commentsKey].GetAll();
                responseModel.LatestComments = comments;
                var totalCommentCount = Client.GetValue(string.Concat(RedisNameConstants.CommentCounterNamePrefix, wallPost.Id));
                var totalLikeCount = Client.GetValue(string.Concat(RedisNameConstants.LikeCounterNamePrefix, wallPost.Id));
                responseModel.CommentCount = !string.IsNullOrWhiteSpace(totalCommentCount) ?
                        Convert.ToInt32(totalCommentCount) : comments.Count;
                responseModel.LikeCount = !string.IsNullOrWhiteSpace(totalLikeCount) ?
                        Convert.ToInt32(totalLikeCount) : 0;
                return responseModel;

            }
            return null;
        }

        public IEnumerable<NewsfeedWallPostModel> GetItems(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                yield return GetItem(id);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }
    }
}
