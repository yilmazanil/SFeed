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
    public class UserNewsfeedService : IUserNewsfeedService, IDisposable
    {
        IUserNewsfeedProvider newsFeedProvider;

        public UserNewsfeedService()
        {
            this.newsFeedProvider = new UserNewsfeedProvider();
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
