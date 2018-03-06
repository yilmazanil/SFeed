using SFeed.Core.Models.Comments;
using SFeed.Core.Models.Newsfeed;
using System;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IWallPostCacheManager : IDisposable
    {
        void AddPost(WallPostCacheModel wallPost);
        void UpdatePost(WallPostCacheModel wallPost);
        void DeletePost(string Id);
        void AddComment(UserCommentModel comment);
        void UpdateComment(string commentBody, string commentId, string postId);
        void DeleteComment(string postId, string commentId);
    }
}
