using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Comments;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository.Caching
{
    public interface ICommentCacheRepository
    {
        void AddItem(CommentCacheModel model);
        bool UpdateItem(CommentUpdateRequest model, DateTime modificationDate);
        void RemoveComment(string postId, long commentId);
        void RemoveAll(int maxRemovalSize);
    }
}
