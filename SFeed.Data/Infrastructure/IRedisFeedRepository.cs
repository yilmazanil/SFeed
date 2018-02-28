using SFeed.Model;
using System.Collections.Generic;

namespace SFeed.Data.Infrastructure
{
    public interface IRedisUserFeedRepository
    { 
        void AddToUserFeeds(IEnumerable<int> userIds, long postId);
        IEnumerable<SocialPostModel> GetUserFeeds(int userId);
        void DeleteFromUserFeeds(long postId, IEnumerable<int> userIds);
    }
}
