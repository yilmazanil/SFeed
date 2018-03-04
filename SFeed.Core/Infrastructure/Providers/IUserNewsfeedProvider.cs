using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserNewsfeedProvider : IDisposable
    {
        void AddToUserFeeds(NewsfeedWallPostModel feedItem, IEnumerable<string> userIds);
        void UpdateFeed(NewsfeedWallPostModel feedItem);
        IEnumerable<NewsfeedResponseItem> GetUserFeed(string userId);
        void RemoveFromFeed(NewsfeedEntry item, IEnumerable<string> userIds);
    }
}
