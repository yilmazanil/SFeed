using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserFollowerService
    {
        void FollowUser(string userIdToFollow);
        void UnFollowUser(string userIdToUnFollow);
        IEnumerable<string> GetFollowers();
        IEnumerable<string> GetFollowers(string userId);
    }
}
