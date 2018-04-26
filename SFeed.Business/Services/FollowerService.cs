using log4net;
using SFeed.Core.Infrastructue.Services;
using System.Collections.Generic;
using System.Linq;
using SFeed.Core.Models.Follower;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;

namespace SFeed.Business.Services
{
    public class FollowerService : IFollowerService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(FollowerService));

        IFollowerProvider followerProvider;

        public FollowerService() : this(new FollowerProvider()) { }

        public FollowerService(IFollowerProvider followerProvider)
        {
            this.followerProvider = followerProvider;
        }

        public void FollowGroup(string followerId, string groupId)
        {
            followerProvider.FollowGroup(followerId, groupId);
        }


        public void UnfollowGroup(string followerId, string groupId)
        {
            followerProvider.UnfollowGroup(followerId, groupId);
        }

        public void FollowUser(string followerId, string userId)
        {
            followerProvider.FollowUser(followerId, userId);
        }

        public void UnfollowUser(string followerId, string userId)
        {
            followerProvider.UnfollowUser(followerId, userId);
        }

        public IEnumerable<string> GetGroupFollowers(string groupId)
        {
            var followers = followerProvider.GetGroupFollowersCached(groupId);
            if (followers == null || !followers.Any())
            {
                followers = followerProvider.GetGroupFollowers(groupId);

                if (followers != null && followers.Any())
                {
                    followerProvider.ResetGroupFollowersCache(groupId, followers);
                }
            }
            return followers;
        }
        public FollowerPagedModel GetGroupFollowersPaged(string groupId, int skip, int size)
        {
            var result = new FollowerPagedModel();
            var followers = GetGroupFollowers(groupId);

            if (followers != null)
            {
                result.TotalCount = followers.Count();
                result.Records = followers.Skip(skip).Take(size);
            }
            return result;
        }
        public IEnumerable<string> GetUserFollowers(string userId)
        {
            var followers = followerProvider.GetUserFollowersCached(userId);
            if (followers == null || !followers.Any())
            {
                followers = followerProvider.GetUserFollowers(userId);

                if (followers != null && followers.Any())
                {
                    followerProvider.ResetUserFollowersCache(userId, followers);
                }
            }
            return followers;
        }
        public FollowerPagedModel GetUserFollowersPaged(string userId, int skip, int size)
        {
            var result = new FollowerPagedModel();
            var followers = GetUserFollowers(userId);

            if (followers != null)
            {
                result.TotalCount = followers.Count();
                result.Records = followers.Skip(skip).Take(size);
            }
            return result;
        }

        public FollowerPagedModel GetFollowedUsersPaged(string userId, int skip, int size)
        {
            var result = new FollowerPagedModel();
            var followers = followerProvider.GetFollowedUsers(userId);

            if (followers != null)
            {
                result.TotalCount = followers.Count();
                result.Records = followers.Skip(skip).Take(size);
            }
            return result;
        }

        public FollowerPagedModel GetFollowedGroupsPaged(string groupId, int skip, int size)
        {
            var result = new FollowerPagedModel();
            var followers = followerProvider.GetFollowedGroups(groupId);

            if (followers != null)
            {
                result.TotalCount = followers.Count();
                result.Records = followers.Skip(skip).Take(size);
            }
            return result;
        }
    }
}
