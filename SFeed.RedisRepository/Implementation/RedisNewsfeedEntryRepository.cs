using SFeed.Core.Infrastructure.Repository.Caching;
using System;
using SFeed.Core.Models.Newsfeed;
using SFeed.RedisRepository.Base;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisNewsfeedEntryRepository : RedisRepositoryBase, INewsfeedCacheRepository
    {

        public void AddEntry(NewsfeedEntry entry)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }

        public void RemoveEntriesByUser(string userId)
        {
            throw new NotImplementedException();
        }

        public void RemoveEntry(NewsfeedEntry entry)
        {
            throw new NotImplementedException();
        }
    }
}
