using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserNewsfeedProvider : IDisposable
    {
        void AddEntry<T>(T feedItem, NewsfeedEntryType entryType, IEnumerable<string> userIds) where T: TypedCacheItemBaseModel;
        void UpdateEntry<T>(T feedItem, NewsfeedEntryType entryType) where T: TypedCacheItemBaseModel;
        IEnumerable<NewsfeedResponseItem> GetNewsfeedByUser(string userId);
        void RemoveFeedFromUsers(NewsfeedEntry item, IEnumerable<string> userIds);
        void Delete(NewsfeedEntry item);
    }
}
