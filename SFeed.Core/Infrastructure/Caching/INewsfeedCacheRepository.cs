using SFeed.Core.Models.Caching;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Caching
{
    public interface INewsfeedCacheRepository
    {
        void PublishEvent(NewsfeedCacheModel entry, IEnumerable<string> followers);
        void RemoveEvent(NewsfeedCacheModel entry, IEnumerable<string> followers);
        void RemovePost(string postId, IEnumerable<string> followers);
        void RemovePostsFromUser(string user, IEnumerable<string> postIds);
        IEnumerable<NewsfeedWallPostModel> GetUserFeed(string userId, int skip, int take);
    }
}
