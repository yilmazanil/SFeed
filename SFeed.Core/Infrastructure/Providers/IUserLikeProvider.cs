using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IEntryLikeProvider
    {
        void LikePost(string postId, string userId);
        void UnlikePost(string postId, string userId);
        IEnumerable<string> GetPostLikes(string postId);
        IEnumerable<string> GetCommentLikes(long commentId);
        void LikeComment(long commentId, string userId);
        void UnlikeComment(long commentId, string userId);
        int GetPostLikeCountCached(string postId);
        int GetCommentLikeCountCached(long commentId);
    }
}
