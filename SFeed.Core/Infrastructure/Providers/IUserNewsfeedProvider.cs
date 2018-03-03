using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserNewsfeedProvider : IDisposable
    {
        void AddToUserFeeds(NewsfeedItemModel feedItem, IEnumerable<string> userIds);
        IEnumerable<NewsfeedItemModel> GetUserFeed(string userId);
    }
}
