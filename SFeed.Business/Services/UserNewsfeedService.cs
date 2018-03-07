using SFeed.Core.Infrastructue.Services;
using System.Collections.Generic;
using System;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;
using SFeed.Core.Models.Newsfeed;
using System.Linq;

namespace SFeed.Business.Services
{
    public sealed class UserNewsfeedService : IUserNewsfeedService, IDisposable
    {
        INewsfeedProvider newsFeedProvider;

        public UserNewsfeedService() : this(new UserNewsfeedProvider())
        {

        }
        public UserNewsfeedService(INewsfeedProvider newsfeedProvider)
        {
            this.newsFeedProvider = newsfeedProvider;
        }

        public void Dispose()
        {
            if (newsFeedProvider != null)
            {
                newsFeedProvider.Dispose();
            }
        }

        public IEnumerable<NewsfeedResponseItem> GetUserNewsfeed(string userId)
        {
            return newsFeedProvider.GetUserNewsfeed(userId);
        }

    }
}
