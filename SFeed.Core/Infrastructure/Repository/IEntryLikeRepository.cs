using SFeed.Core.Models.EntryLike;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface IEntryLikeRepository
    {
        /// <summary>
        /// Adds a like record for a given post, returns true if not already liked
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <param name="userId">User that likes the post</param>
        /// <returns></returns>
        bool LikePost(string postId, string userId);
        /// <summary>
        /// Adds a like record for a given comment, returns true if not already liked
        /// </summary>
        /// <param name="commentId">Id of comment</param>
        /// <param name="userId">User that likes the comment</param>
        /// <returns></returns>
        bool LikeComment(long commentId, string userId);
        /// <summary>
        /// Removes like record for a given post, returns false if no existing record exists
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <param name="userId">User that likes the post</param>
        /// <returns></returns>
        bool UnlikePost(string postId, string userId);
        /// <summary>
        /// Removes like record for a given comment, returns false if no existing record exists
        /// </summary>
        /// <param name="commentId">Id of comment</param>
        /// <param name="userId">User that likes the comment</param>
        /// <returns></returns>
        bool UnlikeComment(long commentId, string userId);
        /// <summary>
        /// Returns the list of users that liked a given post
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <returns></returns>
        IEnumerable<string> GetPostLikes(string postId);
        /// <summary>
        /// Returns the list of users that liked a given comment
        /// </summary>
        /// <param name="commentId">Id of comment</param>
        /// <returns></returns>
        IEnumerable<string> GetCommentLikes(long commentId);
        /// <summary>
        /// Returns the list of users that liked a given post
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <param name="skip">Number of records to skip</param>
        /// <param name="size">Number of records to return</param>
        /// <returns></returns>
        EntryLikePagedModel GetPostLikesPaged(string postId, int skip, int size);
        /// <summary>
        /// Returns the list of users that liked a given comment
        /// </summary>
        /// <param name="commentId">Id of comment</param>
        /// <param name="skip">Number of records to skip</param>
        /// <param name="size">Number of records to return</param>
        /// <returns></returns>
        EntryLikePagedModel GetCommentLikesPaged(long commentId, int skip, int size);
    }
}
