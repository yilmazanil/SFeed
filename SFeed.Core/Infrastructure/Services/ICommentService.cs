using SFeed.Core.Models.Comments;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Services
{
    public interface ICommentService
    {
        string AddComment(CommentCreateModel entry);
        void DeleteComment(string commetId);
        void UpdateComment(CommentCreateModel entry);
        IEnumerable<CommentCreateModel> GetComments(string postId);
    }
}
