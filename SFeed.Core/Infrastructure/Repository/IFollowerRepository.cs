using SFeed.Core.Models.Wall;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository
{
    /// <summary>
    /// Interface for follow/unfollow events of a user
    /// </summary>
    public interface IFollowerRepository
    {
        /// <summary>
        /// Follow activities of a user
        /// </summary>
        /// <param name="userId">UserId to Follow</param>
        /// <param name="followerId">Active UserId</param>
        void FollowUser(string userId, string followerId);
        /// <summary>
        /// Stop following activities of a user
        /// </summary>
        /// <param name="userId">UserId to Unfollow</param>
        /// <param name="followerId">Active UserId</param>
        void UnfollowUser(string userId, string followerId);
        /// <summary>
        /// Follow activities of a group
        /// </summary>
        /// <param name="groupId">GroupId to Follow</param>
        /// <param name="followerId">Active UserId</param>
        void FollowGroup(string groupId, string followerId);
        /// <summary>
        /// Stop following activities of a group
        /// </summary>
        /// <param name="groupId">GroupId to Unfollow</param>
        /// <param name="followerId">Active UserId</param>
        void UnfollowGroup(string groupId, string followerId);
        /// <summary>
        /// Returns the followers of a user
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        IEnumerable<string> GetFollowersUser(string userId);
        /// <summary>
        /// Returns the followers of a group
        /// </summary>
        /// <param name="groupId">GroupId</param>
        /// <returns></returns>
        IEnumerable<string> GetFollowersGroup(string groupId);
        /// <summary>
        /// Returns the users that provided user follows
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        IEnumerable<string> GetFollowedUsers(string userId);
        /// <summary>
        /// Returns the groups that provided user follows
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        IEnumerable<string> GetFollowedGroups(string userId);

    }
}
