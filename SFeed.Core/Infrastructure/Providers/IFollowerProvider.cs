using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IFollowerProvider
    {
        void FollowUser(string followerId, string userId);
        void UnfollowUser(string followerId, string userId);
        void FollowGroup(string followerId, string groupId);
        void UnfollowGroup(string followerId, string groupId);
        IEnumerable<string> GetUserFollowers(string userId);
        IEnumerable<string> GetGroupFollowers(string groupId);
    }
}
