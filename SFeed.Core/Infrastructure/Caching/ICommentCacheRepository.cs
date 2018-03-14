using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Comments;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Caching
{
    /// <summary>
    /// Stores a limited number of comments for a spesific post
    /// </summary>
    public interface ICommentCacheRepository
    {
        /// <summary>
        /// Prepends a comment
        /// </summary>
        /// <param name="model">Comment details</param>
        void AddComment(CommentCacheModel model);
        /// <summary>
        /// Updates a comment if already exists
        /// </summary>
        /// <param name="model">Comment details</param>
        bool UpdateComment(CommentUpdateRequest model, DateTime modificationDate);
        /// <summary>
        /// Removes a comment if already exists
        /// </summary>
        /// <param name="postId">Id of associated post</param>
        /// <param name="commentId">Id of comment</param>
        void RemoveComment(string postId, long commentId);
        /// <summary>
        /// Returns comments for a given post id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        IEnumerable<CommentCacheModel> GetLatestComments(string postId);
        /// <summary>
        /// Returns total number of comments for a given post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        int GetCommentCount(string postId);
    }
}
