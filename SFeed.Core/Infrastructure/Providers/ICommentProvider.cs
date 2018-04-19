using SFeed.Core.Models.Comments;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface ICommentProvider
    {
        /// <summary>
        /// Adds a new comment
        /// </summary>
        /// <param name="request">Comment properties</param>
        /// <returns></returns>
        long AddComment(CommentCreateRequest request);
        /// <summary>
        /// Deletes a comment
        /// </summary>
        /// <param name="postId">Id of associated post</param>
        /// <param name="commentId">Id of comment</param>
        void DeleteComment(string postId, long commentId);
        /// <summary>
        /// Updates a comment
        /// </summary>
        /// <param name="request">Comment properties</param>
        void UpdateComment(CommentUpdateRequest request);
        /// <summary>
        /// Returns comments for a spesific post
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <param name="skip">Number of comments to skip</param>
        /// <param name="size">Number of comments to return</param>
        /// <returns></returns>
        IEnumerable<CommentModel> GetComments(string postId, int skip, int size);
        /// <summary>
        /// Returns comments for a spesific post
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <param name="skip">Number of comments to skip</param>
        /// <param name="size">Number of comments to return</param>
        /// <returns></returns>
        IEnumerable<CommentModel> GetLatestComments(string postId);
        /// <summary>
        /// Returns comments for a spesific post from cache
        /// </summary>
        /// <param name="postId">Id of post</param>
        /// <param name="skip">Number of comments to skip</param>
        /// <param name="size">Number of comments to return</param>
        /// <returns></returns>
        IEnumerable<CommentModel> GetLatestCommentsCached(string postId);
        /// <summary>
        /// Returns a comment with extra properties
        /// </summary>
        /// <param name="postId">Id of associated post</param>
        /// <param name="commentId">Id of comment</param>
        /// <returns></returns>
        CommentModel GetComment(string postId, long commentId);
    }
}
