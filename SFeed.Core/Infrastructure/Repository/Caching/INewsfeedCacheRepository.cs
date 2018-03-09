using SFeed.Core.Models;
using SFeed.Core.Models.Newsfeed;

namespace SFeed.Core.Infrastructure.Repository.Caching
{
    public interface INewsfeedCacheRepository
    {
        void AddEntry(NewsfeedEntry entry);
        void RemoveEntry(NewsfeedEntry entry);
        void RemoveAll();
        void RemoveEntriesByUser(string userId);
    }
}
