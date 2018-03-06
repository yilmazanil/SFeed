using System;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserLikeProvider : IDisposable
    {
        void LikePost(string postId, string userId);
        void UnlikePost(string postId, string userId);
        void LikeComment(long commentId, string userId);
        void UnlikeComment(long commentId, string userId);
    }
}
