﻿using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Comments;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Caching
{
    public interface ICommentCacheRepository
    {
        void AddComment(CommentCacheModel model);
        bool UpdateComment(CommentUpdateRequest model, DateTime modificationDate);
        void RemoveComment(string postId, long commentId);
        void RemoveAllComments(int maxRemovalSize);
        IEnumerable<CommentCacheModel> GetLatestComments(string postId);
        int GetCommentCount(string postId);
    }
}
