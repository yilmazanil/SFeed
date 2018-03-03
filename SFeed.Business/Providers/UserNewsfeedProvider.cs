using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;
using SFeed.Core.Models.Newsfeed;

namespace SFeed.Business.Providers
{
    public class UserNewsfeedProvider : IUserNewsfeedProvider
    {
        ICacheListRepository<NewsfeedItemModel> redisFeedRepo;

        public UserNewsfeedProvider(): this(new RedisUserFeedRepository())
        {

        }
        public UserNewsfeedProvider(ICacheListRepository<NewsfeedItemModel> redisFeedRepo)
        {
            this.redisFeedRepo = redisFeedRepo;
        }
        public void AddToUserFeeds(NewsfeedItemModel feedItem, IEnumerable<string> userIds)
        {
            foreach (var userId in userIds)
            {
                redisFeedRepo.AddToList(userId, feedItem);
            }
        }

        public IEnumerable<NewsfeedItemModel> GetUserFeed(string userId)
        {
            return redisFeedRepo.GetList(userId);
        }

        public void Dispose()
        {
            if (redisFeedRepo != null)
            {
                redisFeedRepo.Dispose();
            }
        }
    }
}
