using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Caching
{
    public interface IFollowerCacheRepository
    {
        void FollowUser(string userId, string followerId);
        void ResetUserFollowers(string userId, IEnumerable<string> followerIds);
        void UnfollowUser(string userId, string followerId);
        void FollowGroup(string groupId, string followerId);
        void ResetGroupFollowers(string groupId, IEnumerable<string> followerIds);
        void UnfollowGroup(string groupId, string followerId);
        IEnumerable<string> GetUserFollowers(string userId);
        IEnumerable<string> GetGroupFollowers(string groupId);
    }
}
