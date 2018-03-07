using SFeed.Core.Models.Comments;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserCommentProvider : IDisposable
    {
        long AddComment(CommentCreateModel request);
        void DeleteComment(string wallPostId, long commentId);
        void UpdateComment(string body, long commentId, string postId);
        IEnumerable<CommentModel> GetComments(string postId);
    }
}
