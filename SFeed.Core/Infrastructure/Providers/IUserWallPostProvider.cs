using SFeed.Core.Models.GroupWall;
using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IWallPostProvider
    {
        /// <summary>
        /// Creates a new post and updates cache
        /// </summary>
        /// <param name="request">Post data</param>
        /// <returns></returns>
        string AddPost(WallPostCreateRequest request);
        /// <summary>
        /// Updates an existing post in both cache and database
        /// </summary>
        /// <param name="model">Post properties</param>
        void UpdatePost(WallPostUpdateRequest model);
        /// <summary>
        /// Removes post from db and cache
        /// </summary>
        /// <param name="postId">Id of post</param>
        void DeletePost(string postId);
        /// <summary>
        /// Returns post with details
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <returns></returns>
        WallPostWithDetailsModel GetPostDetailed(string postId);
        /// <summary>
        /// Returns post
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <returns></returns>
        WallPostModel GetPost(string postId);
        /// <summary>
        /// Returns posts from a user wall with details
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="olderThan">Max post publish date</param>
        /// <param name="size">Number of posts to return</param>
        /// <returns></returns>
        IEnumerable<WallPostWithDetailsModel> GetUserWallDetailed(string userId, DateTime olderThan, int size);
        /// <summary>
        /// Returns posts from a user wall
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="olderThan">Max post publish date</param>
        /// <param name="size">Number of posts to return</param>
        /// <returns></returns>
        IEnumerable<WallPostModel> GetUserWall(string userId, DateTime olderThan, int size);
        /// <summary>
        /// Returns posts from a group wall with details
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="olderThan">Max post publish date</param>
        /// <param name="size">Number of posts to return</param>
        /// <returns></returns>
        IEnumerable<WallPostWithDetailsModel> GetGroupWallDetailed(string groupId, DateTime olderThan, int size);
        /// <summary>
        /// Returns posts from a group wall with details
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="olderThan">Max post publish date</param>
        /// <param name="size">Number of posts to return</param>
        /// <returns></returns>
        IEnumerable<GroupWallResponseModel> GetGroupWall(string groupId, int skip, int size);
    }
}
