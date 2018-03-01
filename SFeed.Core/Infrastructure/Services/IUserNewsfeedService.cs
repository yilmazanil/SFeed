using SFeed.Core.Models;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserNewsfeedService : IDisposable
    {
        void AddToUserFeeds(FeedItemModel feedItem, IEnumerable<string> userIds);
        IEnumerable<FeedItemModel> GetUserFeed(string userId);
    }
}
