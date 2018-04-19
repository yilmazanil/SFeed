namespace SFeed.Core.Infrastructure.Caching
{
    public interface IEntryLikeCacheRepository
    {
        /// <summary>
        /// Increments number of users that likes a post
        /// </summary>
        /// <param name="postId">Id of post</param>
        void IncrementPostLikeCount(string postId);
        /// <summary>
        /// Decrements number of users that likes a post
        /// </summary>
        /// <param name="postId">Id of post</param>
        void DecrementPostLikeCount(string postId);
        /// <summary>
        /// Returns number of users that likes a post
        /// </summary>
        /// <param name="postId">Id of post</param>
        int GetPostLikeCount(string postId);
    }
}
