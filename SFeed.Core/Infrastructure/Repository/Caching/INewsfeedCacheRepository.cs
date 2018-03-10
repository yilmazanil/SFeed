using SFeed.Core.Models;
using SFeed.Core.Models.Newsfeed;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository.Caching
{
    public interface INewsfeedCacheRepository
    {
        void AddEntry(NewsfeedEntry entry, IEnumerable<string> followers);
        void RemoveEntry(NewsfeedEntry entry);
        void RemoveEntriesByUser(string userId, string followerId);
    }
}
