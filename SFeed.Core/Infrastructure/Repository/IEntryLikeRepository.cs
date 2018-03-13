using System.Collections;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface IEntryLikeRepository
    {
        bool LikePost(string postId, string userId);
        bool LikeComment(long commentId, string userId);
        bool UnlikePost(string postId, string userId);
        bool UnlikeComment(long commentId, string userId);
        IEnumerable<string> GetPostLikes(string postId);
        IEnumerable<string> GetCommentLikes(long commentId);
    }
}
