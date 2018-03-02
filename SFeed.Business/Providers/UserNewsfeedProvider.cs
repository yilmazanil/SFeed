using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Models;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;

namespace SFeed.Business.Providers
{
    public class UserNewsfeedProvider : IUserNewsfeedProvider
    {
        ICacheListRepository<FeedItemModel> redisFeedRepo;

        public UserNewsfeedProvider(): this(new RedisUserFeedRepository())
        {

        }
        public UserNewsfeedProvider(ICacheListRepository<FeedItemModel> redisFeedRepo)
        {
            this.redisFeedRepo = redisFeedRepo;
        }
        public void AddToUserFeeds(FeedItemModel feedItem, IEnumerable<string> userIds)
        {
            foreach (var userId in userIds)
            {
                redisFeedRepo.AddToList(userId, feedItem);
            }
        }

        public void DeleteFromFeed(string userId, FeedItemModel feedItem)
        {
            redisFeedRepo.RemoveFromList(userId, feedItem);
        }

        public void Dispose()
        {
            if (redisFeedRepo != null)
            {
                redisFeedRepo.Dispose();
            }
        }

        public IEnumerable<FeedItemModel> GetUserFeed(string userId)
        {
            return redisFeedRepo.GetList(userId);
        }
    }
}
