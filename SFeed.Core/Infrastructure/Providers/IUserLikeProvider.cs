using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserLikeProvider : IDisposable
    {
        void LikePost(string postId, string userId);
        void UnlikePost(string postId, string userId);
        IEnumerable<string> GetPostLikes(string postId);
        void LikeComment(long commentId, string userId);
        void UnlikeComment(long commentId, string userId);
        IEnumerable<string> GetCommentLikes(long commentId);

    }
}
