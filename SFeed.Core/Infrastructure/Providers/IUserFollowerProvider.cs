using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserFollowerProvider : IDisposable
    {
        void FollowUser(string followerId, string userId);
        void UnfollowUser(string followerId, string userId);
        IEnumerable<string> GetFollowers(string userId);
        IEnumerable<string> GetFollowers(IEnumerable<string> userIds);
    }
}
