using SFeed.Core.Models;
using SFeed.Core.Models.Newsfeed;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface INewsfeedProvider
    {
        void AddNewsfeedItem(NewsfeedEntry newsFeedEntry);
        void AddNewsfeedItem(NewsfeedEntry newsFeedEntry, WallOwner wallowner);
        void RemoveNewsfeedItem(NewsfeedEntry newsFeedEntry);
        void RemoveFeedsFromUser(string userId, WallOwner fromWall);
        void RemoveFeedsFromUser(string userId, string fromUser);
    }
}
