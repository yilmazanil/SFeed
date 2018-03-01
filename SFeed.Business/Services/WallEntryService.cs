using SFeed.Core.Infrastructure.Services;
using System.Collections.Generic;
using SFeed.Core.Models;
using SFeed.RedisRepository;
using SFeed.Core.Infrastructue.Repository;

namespace SFeed.Business.Services
{
    public class WallEntryService : IWallEntryService
    {
        ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo;
        public WallEntryService() : this(new RedisWallEntryRepository())
        {
        }

        public WallEntryService(ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo)
        {
            this.wallEntryCacheRepo = wallEntryCacheRepo;
        }

        public void Dispose()
        {
            if (wallEntryCacheRepo != null)
            {
                wallEntryCacheRepo.Dispose();
            }
        }

        public IEnumerable<WallEntryModel> GetEntries(IEnumerable<string> ids)
        {
            return wallEntryCacheRepo.GetByIds(ids);
        }
    }
}
