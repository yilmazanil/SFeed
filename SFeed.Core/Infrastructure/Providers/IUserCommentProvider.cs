using SFeed.Core.Models.Comments;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserCommentProvider
    {
        long AddComment(CommentCreateRequest request);
        void DeleteComment(string postId, long commentId);
        void UpdateComment(CommentUpdateRequest request);
        IEnumerable<CommentModel> GetComments(string postId, DateTime olderThan, int size);
    }
}
