using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserNewsfeedProvider : IDisposable
    {
        void AddNewsfeedItem(NewsfeedEntry entry);
        void DeleteNewsfeedItem(NewsfeedEntry entry);
        IEnumerable<NewsfeedResponseItem> GetUserNewsfeed(string userId);
    }
}
