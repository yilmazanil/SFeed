using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserNewsfeedProvider : IDisposable
    {
        void AddToUserFeeds(NewsfeedEntryModel feedItem, IEnumerable<string> userIds);
        IEnumerable<NewsfeedEntryModel> GetUserFeed(string userId);
        void RemoveFromFeed(NewsfeedEntryModel item, IEnumerable<string> userIds);
    }
}
