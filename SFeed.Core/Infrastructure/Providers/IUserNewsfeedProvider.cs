using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserNewsfeedProvider : IDisposable
    {
        void Add(NewsfeedEntry entry);
        void Delete(NewsfeedEntry entry);
        IEnumerable<NewsfeedResponseItem> GetNewsfeed(string userId);
    }
}
