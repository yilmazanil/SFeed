using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface INewsfeedResponseProvider
    {
        IEnumerable<NewsfeedWallPostModel> GetUserNewsfeed(string userId, int skip, int take);
    }
}
