using SFeed.Core.Models.Comments;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Services
{
    public interface ICommentService
    {
        string AddComment(UserCommentModel entry);
        void DeleteComment(string commetId);
        void UpdateComment(UserCommentModel entry);
        IEnumerable<UserCommentModel> GetComments(string postId);
    }
}
