using SFeed.Core.Models.Follower;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IFollowerService
    {
        /// <summary>
        /// Follow a user
        /// </summary>
        /// <param name="followerId">Follower</param>
        /// <param name="userId">User</param>
        void FollowUser(string followerId, string userId);
        /// <summary>
        /// Unfollow a user
        /// </summary>
        /// <param name="followerId">Follower</param>
        /// <param name="userId">User to Unfollow</param>
        void UnfollowUser(string followerId, string userId);
        /// <summary>
        /// Follow a group
        /// </summary>
        /// <param name="followerId">Follower</param>
        /// <param name="groupId">Group</param>
        void FollowGroup(string followerId, string groupId);
        /// <summary>
        /// Unfollow a group
        /// </summary>
        /// <param name="followerId">Follower</param>
        /// <param name="groupId">Group to Unfollow</param>
        void UnfollowGroup(string followerId, string groupId);
        /// <summary>
        /// Returns followers of a user
        /// </summary>
        /// <param name="userId">User</param>
        /// <returns></returns>
        IEnumerable<string> GetUserFollowers(string userId);
        /// <summary>
        /// Returns followers of a group
        /// </summary>
        /// <param name="groupId">Group</param>
        /// <returns></returns>
        IEnumerable<string> GetGroupFollowers(string groupId);
        /// <summary>
        /// Returns followers of a user paged
        /// </summary>
        /// <param name="userId">User</param>
        /// <param name="skip">Skip size</param>
        /// <param name="size">Return set size</param>
        /// <returns></returns>
        FollowerPagedModel GetUserFollowersPaged(string userId, int skip, int size);
        /// <summary>
        /// Returns followers of a group paged
        /// </summary>
        /// <param name="groupId">Group</param>
        /// <param name="skip">Skip size</param>
        /// <param name="size">Return set size</param>
        /// <returns></returns>
        FollowerPagedModel GetGroupFollowersPaged(string groupId, int skip, int size);
        /// <summary>
        /// Returns the users that a given user follows
        /// </summary>
        /// <param name="userId">User</param>
        /// <param name="skip">Skip size</param>
        /// <param name="size">Size of return set</param>
        /// <returns></returns>
        FollowerPagedModel GetFollowedUsersPaged(string userId, int skip, int size);
        /// <summary>
        /// Returns the groups that a given user follows
        /// </summary>
        /// <param name="userId">User</param>
        /// <param name="skip">Skip size</param>
        /// <param name="size">Size of return set</param>
        /// <returns></returns
        FollowerPagedModel GetFollowedGroupsPaged(string groupId, int skip, int size);
    }
}
