using SFeed.Core.Infrastructure.Services;
using System.Collections.Generic;
using SFeed.Core.Models;
using SFeed.RedisRepository;
using SFeed.Core.Infrastructue.Repository;
using SFeed.SqlRepository;
using System;

namespace SFeed.Business.Services
{
    public class WallEntryService : IWallEntryService
    {
        ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo;
        IRepository<WallEntry> wallEntryRepo;

        public WallEntryService() : this(
            new RedisWallEntryRepository(),
            new WallEntryRepository())
        {
        }

        public WallEntryService(
            ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo,
            IRepository<WallEntry> wallEntryRepo)
        {
            this.wallEntryCacheRepo = wallEntryCacheRepo;
            this.wallEntryRepo = wallEntryRepo;

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

        public void Delete(string postId)
        {
            var postGuid = Guid.Parse(postId);
            wallEntryRepo.Delete(p => p.Id == postGuid);
            wallEntryRepo.CommitChanges();
            wallEntryCacheRepo.RemoveItem(postId);
        }
    }
}
