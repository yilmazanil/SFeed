using System;
using System.Collections.Generic;
using SFeed.Core.Infrastructue.Services;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;
using SFeed.Core.Models;

namespace SFeed.Business.Services
{
    public sealed class UserFollowerService : IUserFollowerService, IDisposable
    {
        private readonly IFollowerProvider userFollowerProvider;

        public UserFollowerService() :
            this(new FollowerProvider())
        {

        }

        public UserFollowerService(IFollowerProvider userFollowerProvider)
        {
            this.userFollowerProvider = userFollowerProvider;
        }

        public void FollowUser(string activeUserId, string userIdToFollow)
        {
            userFollowerProvider.FollowUser(activeUserId, userIdToFollow);

        }
        public void UnFollowUser(string activeUserId, string userIdToUnFollow)
        {
            userFollowerProvider.UnfollowUser(activeUserId, userIdToUnFollow);
        }

    }
}
