using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserFollowerService : IDisposable
    {
        void FollowUser(string activeUserId, string userIdToFollow);
        void UnFollowUser(string activeUserId, string userIdToUnFollow);
        IEnumerable<string> GetFollowers(string userId);
    }
}
