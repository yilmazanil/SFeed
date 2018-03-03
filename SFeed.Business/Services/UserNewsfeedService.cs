using SFeed.Core.Infrastructue.Services;
using System.Collections.Generic;
using System;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;
using SFeed.Core.Models.Newsfeed;

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

        public IEnumerable<NewsfeedItemModel> GetUserFeed(string userId)
        {
            return newsFeedProvider.GetUserFeed(userId);
        }
    }
}
