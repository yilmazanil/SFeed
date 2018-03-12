namespace SFeed.Core.Infrastructure.Repository.Caching
{
    public interface IEntryLikeCacheRepository
    {
        void IncrementPostLikeCount(string postId);
        void DecrementPostLikeCount(string postId);
        void IncrementCommentLikeCount(long commentId);
        void DecrementCommentLikeCount(long commentId);
        int GetCommentLikeCount(long commentId);
        int GetPostLikeCount(string postId);
    }
}
