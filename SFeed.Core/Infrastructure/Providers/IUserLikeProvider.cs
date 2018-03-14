using SFeed.Core.Models.EntryLike;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IEntryLikeProvider
    {
        /// <summary>
        /// Add a record indicating that a user likes a post
        /// </summary>
        /// <param name="postId">Id of post </param>
        /// <param name="userId">Id of user</param>
        void LikePost(string postId, string userId);
        /// <summary>
        /// Remove record indicating that a user likes a post
        /// </summary>
        /// <param name="postId">Id of post </param>
        /// <param name="userId">Id of user</param>
        void UnlikePost(string postId, string userId);
        /// <summary>
        /// Returns users that liked a given post
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <returns></returns>
        IEnumerable<string> GetPostLikes(string postId);
        /// <summary>
        /// Returns users that liked a given comment
        /// </summary>
        /// <param name="commentId">Id of comment</param>
        /// <returns></returns>
        IEnumerable<string> GetCommentLikes(long commentId);
        /// <summary>
        /// Returns users that liked a given post paged
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <param name="skip">Number of records to skip</param>
        /// <param name="size">Number of records to return</param>
        /// <returns></returns>
        EntryLikePagedModel GetPostLikesPaged(string postId, int skip, int size);
        /// <summary>
        /// Returns users that liked a given comment paged
        /// </summary>
        /// <param name="commentId">Id of comment</param>
        /// <param name="skip">Number of records to skip</param>
        /// <param name="size">Number of records to return</param>
        /// <returns></returns>
        EntryLikePagedModel GetCommentLikesPaged(long commentId, int skip, int size);
        /// <summary>
        /// Add a record indicating that a user likes a comment
        /// </summary>
        /// <param name="commentId">Id of comment </param>
        /// <param name="userId">Id of user</param>
        void LikeComment(long commentId, string userId);
        /// <summary>
        /// REmove record indicating that a user likes a comment
        /// </summary>
        /// <param name="commentId">Id of comment </param>
        /// <param name="userId">Id of user</param>
        void UnlikeComment(long commentId, string userId);
        /// <summary>
        /// Returns number of users that liked a given post
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <returns></returns>
        int GetPostLikeCountCached(string postId);
        /// <summary>
        /// Returns number of users that liked a given comment
        /// </summary>
        /// <param name="commentId">Id of post</param>
        /// <returns></returns>
        int GetCommentLikeCountCached(long commentId);
    }
}
