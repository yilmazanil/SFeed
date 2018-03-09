using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository.Caching
{
    public interface IFollowerCacheRepository
    {
        void FollowUser(string userId, string followerId);
        void UnfollowUser(string userId, string followerId);
        void FollowGroup(string groupId, string followerId);
        void UnfollowGroup(string groupId, string followerId);
        IEnumerable<string> GetUserFollowers(string userId);
        IEnumerable<string> GetGroupFollowers(string groupId);
    }
}
