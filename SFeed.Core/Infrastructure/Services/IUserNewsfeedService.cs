using SFeed.Core.Models;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserNewsfeedService
    {
        void AddToUserFeeds(FeedItemModel feedItem, IEnumerable<string> userIds);
        IEnumerable<WallEntryModel> GetUserFeed(string userId);
    }
}
