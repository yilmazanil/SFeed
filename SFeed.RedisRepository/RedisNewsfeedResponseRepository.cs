using ServiceStack.Redis;
using SFeed.Core.Infrastructure.Repository;
using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var wallPostClient = Client.As<WallPostCacheModel>();
            var wallPost = wallPostClient.GetValue(string.Concat("wallPost:", id));
            var responseModel = new NewsfeedWallPostModel();

            if (wallPost != null)
            {
                responseModel.Body = wallPost.Body;
                responseModel.Id = wallPost.Id;

                var commentClient = Client.As<CommentCacheModel>();
                var commentSearchPattern = string.Concat("postComment:", id, ":*");
                var keys = Client.ScanAllKeys(commentSearchPattern).ToList();;
                var comments = commentClient.GetValues(keys);
                responseModel.LatestComments = comments;

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
