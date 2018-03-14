using SFeed.Core.Models.Comments;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface ICommentRepository
    {
        /// <summary>
        /// Saves a new comment
        /// </summary>
        /// <param name="model">Comment details</param>
        /// <returns></returns>
        CommentCreateResponse SaveComment(CommentCreateRequest model);
        /// <summary>
        /// Updates an existing comment, returns modified date if succeds
        /// </summary>
        /// <param name="model">Comment details</param>
        /// <returns></returns>
        DateTime? UpdateComment(CommentUpdateRequest model);
        /// <summary>
        /// Returns comment with extra properties
        /// </summary>
        /// <param name="postId">Id of associated post</param>
        /// <param name="commentId">Id of comment</param>
        /// <returns></returns>
        CommentDetailedModel GetCommentWithDetails(string postId, long commentId);
        /// <summary>
        /// Returns comments
        /// </summary>
        /// <param name="postId">Id of associated post</param>
        /// <param name="commentId">Id of comment</param>
        /// <returns></returns>
        CommentModel GetComment(string postId, long commentId);
        /// <summary>
        /// Removes a comment with specified id
        /// </summary>
        /// <param name="commentId">Id of comment</param>
        /// <returns></returns>
        void RemoveComment(long commentId);
        /// <summary>
        /// Returns all comments of a given post paged
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <param name="skip">Number of comments to skip</param>
        /// <param name="size">Number of comments to return</param>
        /// <returns></returns>
        IEnumerable<CommentDetailedModel> GetPagedComments(string postId, int skip, int size);
    }
}
