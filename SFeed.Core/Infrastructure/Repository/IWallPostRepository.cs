using SFeed.Core.Models.Wall;
using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface IWallPostRepository
    {
        /// <summary>
        /// Saves a new post with provided properties
        /// </summary>
        /// <param name="model">Wall post properties</param>
        /// <returns></returns>
        WallPostCreateResponse SavePost(WallPostCreateRequest model);
        /// <summary>
        /// Updates provided post, returns modiifcation date if succeeds
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Modification date if succeeds</returns>
        DateTime? UpdatePost(WallPostUpdateRequest model);
        /// <summary>
        /// Returns post with details for provied Id
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <returns></returns>
        WallPostDetailedModel GetPostDetailed(string postId);
        /// <summary>
        /// Returns post for provied Id
        /// </summary>
        /// <param name="postId">Id of Post</param>
        /// <returns></returns>
        WallPostModel GetPost(string postId);
        /// <summary>
        /// Deletes post with provided Id
        /// </summary>
        /// <param name="postId">Id of Post</param>
        /// <returns></returns>
        bool RemovePost(string postId);
        /// <summary>
        /// Returns posts with details belong to a user wall
        /// </summary>
        /// <param name="userId">Id of User</param>
        /// <param name="olderThan">Filter for CreatedDate</param>
        /// <param name="size">Number of items to return</param>
        /// <returns></returns>
        IEnumerable<WallPostDetailedModel> GetUserWallDetailed(string userId, DateTime olderThan, int size);
        /// <summary>
        /// Returns posts belong to a user wall
        /// </summary>
        /// <param name="userId">Id of User</param>
        /// <param name="olderThan">Filter for CreatedDate</param>
        /// <param name="size">Number of items to return</param>
        /// <returns></returns>
        IEnumerable<WallPostModel> GetUserWall(string userId, DateTime olderThan, int size);
        /// <summary>
        /// Returns posts with details belong to a group wall
        /// </summary>
        /// <param name="groupId">Id of group</param>
        /// <param name="olderThan">Filter for CreatedDate</param>
        /// <param name="size">Number of items to return</param>
        /// <returns></returns>
        IEnumerable<WallPostDetailedModel> GetGroupWallDetailed(string groupId, DateTime olderThan, int size);
        // <summary>
        /// Returns posts belong to a group wall
        /// </summary>
        /// <param name="groupId">Id of group</param>
        /// <param name="olderThan">Filter for CreatedDate</param>
        /// <param name="size">Number of items to return</param>
        /// <returns></returns>
        IEnumerable<WallPostModel> GetGroupWall(string groupId, DateTime olderThan, int size);
        /// <summary>
        /// Returns Ids of all posts from a given user accross all walls
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<string> GetUserPostIds(string userId);
        /// <summary>
        /// Returns Ids of posts belonging to a wall
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<string> GetWallPostIds(WallModel targetWall);
    }
}
