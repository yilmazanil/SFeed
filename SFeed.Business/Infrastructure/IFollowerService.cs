using System.Collections.Generic;

namespace SFeed.Business.Infrastructure
{
    public interface IFollowerService
    {
        void FollowUser(int activeUserId, int userToFollow);
        void UnFollowUser(int activeUserId, int userToUnFollow);
        IEnumerable<int> GetFollowers(int userId);
    }
}
