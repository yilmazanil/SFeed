using SFeed.Core.Models.Comments;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface ICommentProvider
    {
        string AddComment(UserCommentModel entry);
        void DeleteComment(string commentId);
        void UpdateComment(UserCommentModel entry);
        IEnumerable<UserCommentModel> GetComments(string postId);
    }
}
