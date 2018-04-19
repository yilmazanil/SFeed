using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserNewsfeedService : IDisposable
    {
        IEnumerable<NewsfeedWallPostModel> GetUserNewsfeed(string userId);
    }
}
