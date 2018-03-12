using SFeed.Core.Models;
using SFeed.Core.Models.Newsfeed;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface INewsfeedProvider
    {
        void AddNewsfeedItem(NewsfeedItem newsFeedEntry);
        void RemoveNewsfeedItem(NewsfeedItem newsFeedEntry);
        void RemovePost(NewsfeedItem newsFeedEntry);
    }
}
