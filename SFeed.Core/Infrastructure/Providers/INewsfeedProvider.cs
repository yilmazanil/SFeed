using SFeed.Core.Models;
using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Newsfeed;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface INewsfeedProvider
    {
        void AddNewsfeedItem(NewsfeedItem newsFeedEntry);
        void RemoveNewsfeedItem(NewsfeedItem newsFeedEntry);
        void RemovePost(NewsfeedItem newsFeedEntry);
        IEnumerable<NewsfeedWallPostModel> GetUserFeed(string userId, int skip, int take);
    }
}
