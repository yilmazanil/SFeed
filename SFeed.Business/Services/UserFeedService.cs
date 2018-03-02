using SFeed.Core.Infrastructue.Repository;
using SFeed.Core.Infrastructue.Services;
using SFeed.Core.Models;
using SFeed.RedisRepository;
using System.Collections.Generic;
using System;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;

namespace SFeed.Business.Services
{
    public class UserFeedService : IUserNewsfeedService, IDisposable
    {
        IUserNewsfeedProvider newsFeedProvider;

        public UserFeedService()
        {
            this.newsFeedProvider = new UserNewsfeedProvider();
        }

        public void AddToUserFeeds(FeedItemModel feedItem, IEnumerable<string> userIds)
        {
            newsFeedProvider.AddToUserFeeds(feedItem, userIds);
        }

        public void DeleteFromFeed(string userId, FeedItemModel feedItem)
        {
            newsFeedProvider.DeleteFromFeed(userId, feedItem);
        }

        public void Dispose()
        {
            if (newsFeedProvider != null)
            {
                newsFeedProvider.Dispose();
            }
        }

        public IEnumerable<FeedItemModel> GetUserFeed(string userId)
        {
            return newsFeedProvider.GetUserFeed(userId);
        }
    }
}
