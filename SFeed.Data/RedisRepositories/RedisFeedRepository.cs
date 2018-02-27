using SFeed.Data.Infrastructure;
using System.Collections.Generic;

namespace SFeed.Data.RedisRepositories
{
    public class RedisFeedRepository : RedisListRepositoryBase<int, long>, IRedisUserFeedRepository
    {
        protected override string ListPrefix => "userFeed";


        public void AddToUserFeeds(IEnumerable<int> userIds, long postId)
        {
            using (var redis = RedisConnectionHelper.ClientManager.GetClient())
            {
                var feedlist = redis.As<long>();

                foreach (var userId in userIds)
                {
                    feedlist.Lists[ListPrefix + userId].Add(postId);
                }
            }
        }

        public IEnumerable<SocialPost> GetUserFeeds(int userId)
        {
            using (var redis = RedisConnectionHelper.ClientManager.GetClient())
            {
                var feedlist = redis.As<long>();

                var postIds = feedlist.Lists[ListPrefix + userId].GetAll();

                var posts = redis.As<SocialPost>();

                return posts.GetByIds(postIds);
            }
        }
    }
}
