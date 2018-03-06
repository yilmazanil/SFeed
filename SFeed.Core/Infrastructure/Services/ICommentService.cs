using SFeed.Core.Models.Comments;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Services
{
    public interface ICommentService
    {
        string AddComment(CommentRequestModel entry);
        void DeleteComment(string commetId);
        void UpdateComment(CommentRequestModel entry);
        IEnumerable<CommentRequestModel> GetComments(string postId);
    }
}
