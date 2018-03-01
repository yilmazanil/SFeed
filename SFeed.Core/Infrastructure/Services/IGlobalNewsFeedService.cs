using SFeed.Core.Models;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IGlobalNewsfeedService : IDisposable
    {
        void AddToFeeds(FeedItemModel feedItem);
        IEnumerable<FeedItemModel> GetFeeds();
    }
}
