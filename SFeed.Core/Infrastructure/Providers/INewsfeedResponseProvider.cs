using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface INewsfeedResponseProvider
    {
        IEnumerable<NewsfeedResponseItem> GetUserNewsfeed(string userId);
    }
}
