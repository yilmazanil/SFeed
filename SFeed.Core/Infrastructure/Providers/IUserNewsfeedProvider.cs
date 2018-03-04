using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserNewsfeedProvider : IDisposable
    {
        void AddToUserFeeds<T>(T feedItem, NewsfeedEntryType entryType, IEnumerable<string> userIds) where T: TypedCacheItemBaseModel;
        void UpdateFeed<T>(T feedItem, NewsfeedEntryType entryType) where T: TypedCacheItemBaseModel;
        IEnumerable<NewsfeedResponseItem> GetUserFeed(string userId);
        void RemoveFromFeed(NewsfeedEntry item, IEnumerable<string> userIds);
    }
}
