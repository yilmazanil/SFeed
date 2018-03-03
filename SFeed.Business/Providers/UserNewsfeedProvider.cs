using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;
using SFeed.Core.Models.Newsfeed;

namespace SFeed.Business.Providers
{
    public class UserNewsfeedProvider : IUserNewsfeedProvider
    {
        ICacheListRepository<NewsfeedEntryModel> redisFeedRepo;

        public UserNewsfeedProvider() : this(new RedisUserFeedRepository())
        {

        }
        public UserNewsfeedProvider(ICacheListRepository<NewsfeedEntryModel> redisFeedRepo)
        {
            this.redisFeedRepo = redisFeedRepo;
        }
        public void AddToUserFeeds(NewsfeedEntryModel feedItem, IEnumerable<string> userIds)
        {
            foreach (var userId in userIds)
            {
                redisFeedRepo.PrependToList(userId, feedItem);
            }
        }

        public IEnumerable<NewsfeedEntryModel> GetUserFeed(string userId)
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

        public void RemoveFromFeed(NewsfeedEntryModel item, IEnumerable<string> userIds)
        {
            foreach (var userId in userIds)
            {
                redisFeedRepo.RemoveFromList(userId, item);
            }
        }
    }
}
