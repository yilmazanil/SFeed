using SFeed.Core.Models;
using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface INewsfeedProvider : INewsfeedResponseProvider, IDisposable
    {
        void AddNewsfeedItem(NewsfeedEntry newsFeedEntry);
        void AddNewsfeedItem(NewsfeedEntry newsFeedEntry, List<Actor> actors);
        void RemoveNewsfeedItem(NewsfeedEntry newsFeedEntry);
        void RemoveNewsfeedItem(NewsfeedEntry newsFeedEntry, List<Actor> actors);
    }
}
