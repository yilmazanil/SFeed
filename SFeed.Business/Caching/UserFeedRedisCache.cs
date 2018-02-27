using SFeed.Data.Infrastructure;
using SFeed.Model;
using System.Collections.Generic;

namespace SFeed.Business.Caching
{
    public class UserFeedRedisCache
    {
        private static readonly string listKey = "userFeed";

        public static void AddFeed(IEnumerable<int> userIds, long postId)
        {
            using (var redis = RedisConnectionHelper.ClientManager.GetClient())
            {
                var feedlist = redis.As<long>();

                foreach (var userId in userIds)
                {
                    feedlist.Lists[listKey + userId].Add(postId);
                }
            }
        }

        public static IEnumerable<SocialPostViewModel> GetFeed(int userId)
        {
            using (var redis = RedisConnectionHelper.ClientManager.GetClient())
            {
                var feedlist = redis.As<long>();

                var postIds = feedlist.Lists[listKey + userId].GetAll();

                var posts = redis.As<SocialPostViewModel>();

                return posts.GetByIds(postIds);
            }

        }
    }
}
