using SFeed.Core.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using SFeed.Core.Models;
using SFeed.Core.Infrastructue.Repository;
using SFeed.SqlRepository;
using SFeed.RedisRepository;
using AutoMapper;

namespace SFeed.Business.Providers
{
    public class UserWallEntryProvider : IUserWallEntryProvider
    {
        private IRepository<UserWall> userWallRepository;
        private IRepository<WallEntry> wallEntryRepository;
        private ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo;

        public UserWallEntryProvider() : this(
            new RedisWallEntryRepository(),
            new UserWallRepository(),
            new WallEntryRepository())
        {
        }

        public UserWallEntryProvider(
            ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo,
            IRepository<UserWall> wallEntryRepo,
            IRepository<WallEntry> wallEntryRepository)
        {
            this.wallEntryCacheRepo = wallEntryCacheRepo;
            this.userWallRepository = wallEntryRepo;
            this.wallEntryRepository = wallEntryRepository;

        }

        public string AddEntry(WallEntryModel model, string wallOwnerId)
        {
            var dbEntry = new WallEntry { Body = model.Body, CreatedBy = model.CreatedBy, IsDeleted = false, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };
            wallEntryRepository.Add(dbEntry);
            wallEntryRepository.CommitChanges();
            userWallRepository.Add(new UserWall { UserId = wallOwnerId, WallEntryId = dbEntry.Id });
            model.Id = dbEntry.Id;
            wallEntryCacheRepo.AddItem(model);
            return dbEntry.Id.ToString();
        }

        public void DeleteEntry(string postId)
        {
            var postGuid = Guid.Parse(postId);
            wallEntryRepository.Delete(p => p.Id == postGuid);
            wallEntryRepository.CommitChanges();
            wallEntryCacheRepo.RemoveItem(postId);
        }

        public void Dispose()
        {
            userWallRepository.Dispose();
            wallEntryRepository.Dispose();
            wallEntryCacheRepo.Dispose();
        }

        public IEnumerable<WallEntryModel> GetEntries(IEnumerable<string> postIds)
        {
            return wallEntryCacheRepo.GetByIds(postIds);
        }

        public WallEntryModel GetEntry(string postId)
        {
            var entry = wallEntryCacheRepo.GetItem(postId);
            if (entry == null)
            {
                var postGuid = Guid.Parse(postId);
                var dbEntry = wallEntryRepository.Get(e => e.Id == postGuid && e.IsDeleted == false);
                if (dbEntry != null)
                {
                    entry = Mapper.Map<WallEntryModel>(dbEntry);
                    wallEntryCacheRepo.AddItem(entry);
                }
            }
            return entry;
        }

        public IEnumerable<WallEntryModel> GetUserWall(string wallOwnerId)
        { 
            //TODO:Use SP & Introduce new interface
            var postIds = userWallRepository.GetMany(p => p.UserId == wallOwnerId).Select(p => p.WallEntryId);
            var results = wallEntryRepository.GetMany(p => p.IsDeleted == false &&  postIds.Contains(p.Id));
            return Mapper.Map<IEnumerable<WallEntryModel>>(results);
        }

        public void UpdateEntry(WallEntryModel model)
        {
            var dbEntry = new WallEntry {
                Body = model.Body, ModifiedDate = DateTime.Now,
                Id = model.Id
            };
            wallEntryRepository.Update(dbEntry);
            wallEntryRepository.CommitChanges();
            wallEntryCacheRepo.UpdateItem(model.Id, model);
        }

        public IEnumerable<WallEntryModel> GetEntries(string userId)
        {
            var posts = wallEntryRepository.GetMany(p => p.CreatedBy == userId && p.IsDeleted == false);
            return Mapper.Map <IEnumerable<WallEntryModel>>(posts);
        }
    }
}
