using SFeed.Core.Models.Newsfeed;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Caching
{
    public interface INewsfeedCacheRepository
    {
        /// <summary>
        /// Adds an event to newsfeed of followers
        /// </summary>
        /// <param name="entry">Event details</param>
        /// <param name="followers">Followers</param>
        void AddEvent(NewsfeedEventModel entry, IEnumerable<string> followers);
        /// <summary>
        /// Removes an event from newsfeed of followers
        /// </summary>
        /// <param name="entry">Event details</param>
        /// <param name="followers">Followers</param>
        void RemoveEvent(NewsfeedEventModel entry, IEnumerable<string> followers);
        /// <summary>
        /// Removes all posts with provided ids from a user newsfeed
        /// </summary>
        /// <param name="user">User to remove from</param>
        /// <param name="postIds">Post ids</param>
        //void RemovePostsFromUser(string user, IEnumerable<string> postIds);
    }
}
