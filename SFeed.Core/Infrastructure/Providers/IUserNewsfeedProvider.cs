using SFeed.Core.Models;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserNewsfeedProvider : IDisposable
    {
        void AddToUserFeeds(FeedItemModel feedItem, IEnumerable<string> userIds);
        IEnumerable<FeedItemModel> GetUserFeed(string userId);
        void DeleteFromFeed(string userId, FeedItemModel feedItem);
    }
}
