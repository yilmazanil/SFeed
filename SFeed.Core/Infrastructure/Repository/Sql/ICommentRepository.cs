using SFeed.Core.Models.Comments;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository.Sql
{
    public interface ICommentRepository
    {
        CommentCreateResponse SaveItem(CommentCreateRequest model);
        DateTime? UpdateItem(CommentUpdateRequest model);
        CommentModel GetComment(string postId, long commentId);
        void RemoveItem(string postId, long commentId);
        IEnumerable<CommentModel> GetComments(string postId, DateTime olderThan, int size);
    }
}
