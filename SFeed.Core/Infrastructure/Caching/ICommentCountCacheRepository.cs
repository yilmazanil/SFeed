namespace SFeed.Core.Infrastructure.Caching
{
    public interface ICommentCountCacheRepository
    {
        /// <summary>
        /// Increments comment count of a given post
        /// </summary>
        /// <param name="postId">Id of post</param>
        void Increment(string postId);
        /// <summary>
        /// Decrements comment count of a given post
        /// </summary>
        /// <param name="postId">Id of post</param>
        void Decrement(string postId);
        /// <summary>
        /// Removes comment counter for a given post
        /// </summary>
        /// <param name="postId">Id of post</param>
        void Remove(string postId);
        /// <summary>
        /// Returns total number of comments for a given post
        /// </summary>
        /// <param name="postId">Id of post</param>
        int GetCommentCount(string postId);
    }
}
