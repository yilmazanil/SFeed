using SFeed.Model;
using System.Collections.Generic;

namespace SFeed.Business.Infrastructure
{
    public interface IUserFeedService
    {
        void AddToUserFeeds(long postId, IEnumerable<int> users);
        IEnumerable<SocialPostModel> GetUserFeed(int userId);
        void DeleteFromFeeds(long postId);
    }
}
