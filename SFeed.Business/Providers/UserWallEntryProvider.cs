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
        private IRepository<UserWall> userWallRepo;
        private IRepository<WallEntry> wallEntryRepo;
        private ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo;

        public UserWallEntryProvider() : this(
            new RedisWallEntryRepository(),
            new UserWallRepository(),
            new WallEntryRepository())
        {

        }

        public UserWallEntryProvider(
            ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo,
            IRepository<UserWall> userWallRepo,
            IRepository<WallEntry> wallEntryRepo)
        {
            this.wallEntryCacheRepo = wallEntryCacheRepo;
            this.userWallRepo = userWallRepo;
            this.wallEntryRepo = wallEntryRepo;

        }

        public string AddEntry(WallEntryModel model, string wallOwnerId)
        {
            var dbEntry = new WallEntry { Body = model.Body, CreatedBy = model.CreatedBy, IsDeleted = false, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };

            //Save Post
            wallEntryRepo.Add(dbEntry);
            wallEntryRepo.CommitChanges();

            var postId = dbEntry.Id;

            if (postId != Guid.Empty)
            {
                //Update UserWall
                userWallRepo.Add(new UserWall { UserId = wallOwnerId, WallEntryId = dbEntry.Id });
                userWallRepo.CommitChanges();

                //Update Cache
                model.Id = postId;
                wallEntryCacheRepo.AddItem(model);

                return model.Id.ToString();
            }
            return string.Empty;
        }

        public void UpdateEntry(WallEntryModel model)
        {
            var dbEntry = new WallEntry
            {
                Body = model.Body,
                ModifiedDate = DateTime.Now,
                Id = model.Id
            };
            //Update DB
            wallEntryRepo.Update(dbEntry);
            wallEntryRepo.CommitChanges();
            //Update Cache
            wallEntryCacheRepo.UpdateItem(model.Id, model);
        }

        public void DeleteEntry(string postId)
        {
            var postGuid = Guid.Parse(postId);
            if (postGuid != Guid.Empty)
            {
                //Mark as deleted
                wallEntryRepo.Delete(p => p.Id == postGuid);
                wallEntryRepo.CommitChanges();
                //Remove from Cache
                wallEntryCacheRepo.RemoveItem(postId);
            }
        }
        public WallEntryModel GetEntry(string postId)
        {
            //Check cache
            var entry = wallEntryCacheRepo.GetItem(postId);
            if (entry == null)
            {
                //Check db
                var postGuid = Guid.Parse(postId);
                var dbEntry = wallEntryRepo.Get(e => e.Id == postGuid && e.IsDeleted == false);
                if (dbEntry != null)
                {
                    entry = Mapper.Map<WallEntryModel>(dbEntry);
                    //Update Cache
                    wallEntryCacheRepo.AddItem(entry);
                }
            }
            return entry;
        }

        public IEnumerable<WallEntryModel> GetUserWall(string wallOwnerId)
        {
            //TODO:Use SP & Introduce new interface
            var postIds = userWallRepo.GetMany(p => p.UserId == wallOwnerId).Select(p => p.WallEntryId);
            var results = wallEntryRepo.GetMany(p => p.IsDeleted == false && postIds.Contains(p.Id));
            return Mapper.Map<IEnumerable<WallEntryModel>>(results);
        }

        public IEnumerable<WallEntryModel> GetEntries(IEnumerable<string> postIds)
        {
            //Retrieve from cache
            return wallEntryCacheRepo.GetByIds(postIds);
        }

        public void Dispose()
        {
            if (userWallRepo != null)
            {
                userWallRepo.Dispose();
            }
            if (wallEntryRepo != null)
            {
                wallEntryRepo.Dispose();
            }

            if (wallEntryCacheRepo != null)
            {
                wallEntryCacheRepo.Dispose();
            }
        }

    }
}
