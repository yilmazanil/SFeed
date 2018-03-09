using SFeed.Core.Models;
using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface INewsfeedProvider : INewsfeedResponseProvider, IDisposable
    {
        void AddNewsfeedItem(NewsfeedEntry newsFeedEntry);
        void AddNewsfeedItem(NewsfeedEntry newsFeedEntry, List<WallOwner> actors);
        void RemoveNewsfeedItem(string actionBy, Predicate<NewsfeedEntry> where);
        void RemoveNewsfeedItem(List<WallOwner> actors, Predicate<NewsfeedEntry> where);
    }
}
