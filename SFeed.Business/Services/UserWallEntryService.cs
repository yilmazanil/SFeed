using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SFeed.Core.Infrastructue.Services;
using SFeed.SqlRepository;
using SFeed.Core.Models;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;

namespace SFeed.Business.Services
{
    public class UserWallEntryService : IUserWallEntryService, IDisposable
    {
        IRepository<WallEntry> wallEntryRepo;
        IRepository<UserWallEntry> userWallEntryRepo;
        ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo;

        public UserWallEntryService(IRepository<WallEntry> wallEntryRepo,
            IRepository<UserWallEntry> userWallEntryRepo,
            ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo)
        {
            this.wallEntryRepo = wallEntryRepo;
            this.userWallEntryRepo = userWallEntryRepo;
            this.wallEntryCacheRepo = wallEntryCacheRepo;
        }

        public UserWallEntryService()
        {
            this.wallEntryRepo = new WallEntryRepository();
            this.userWallEntryRepo = new UserWallEntryRepository();
            this.wallEntryCacheRepo = new RedisWallEntryRepository();
        }
        public void Delete(Guid postId)
        {
            userWallEntryRepo.Delete(p => p.WallEntryId == postId);
            userWallEntryRepo.CommitChanges();
            wallEntryRepo.Delete(p => p.Id == postId);
            wallEntryRepo.CommitChanges();
        }

        public IEnumerable<WallEntryModel> GetUserWall(string userId)
        {
            //TODO:Use SP & Introduce new interface
            var postIds = userWallEntryRepo.GetMany(p => p.UserId == userId).Select(p=>p.WallEntryId);
            var results=  wallEntryRepo.GetMany(p => postIds.Contains(p.Id));
            return Mapper.Map<IEnumerable<WallEntryModel>>(results);
        }

        public Guid PublishEntryToUserWall(WallEntryModel entry, string wallOwnerUserId)
        {
            var dbEntry = new WallEntry { Body = entry.Body, CreatedBy = entry.CreatedBy, IsDeleted = false, CreatedDate = DateTime.Now };
            var userWallEntry = new UserWallEntry { UserId = wallOwnerUserId };
            Guid postId = Guid.Empty;

            try
            {
                wallEntryRepo.Add(dbEntry);
                wallEntryRepo.CommitChanges();

                if (dbEntry.Id != Guid.Empty)
                {
                    userWallEntryRepo.Add(userWallEntry);
                    userWallEntryRepo.CommitChanges();

                    postId = dbEntry.Id;
                    entry.Id = postId;
                    userWallEntry.WallEntryId = dbEntry.Id;
                    wallEntryCacheRepo.AddItem(entry);
                }

            }
            catch (Exception)
            {
                if (postId != Guid.Empty)
                {
                    userWallEntryRepo.Delete(p => p.WallEntryId == postId);
                    wallEntryRepo.Delete(p => p.Id == postId);
                    wallEntryCacheRepo.RemoveItem(postId);

                }
            }
            return postId;
        }

        public void Dispose()
        {
            if (wallEntryRepo != null)
            {
                wallEntryRepo.Dispose();
            }

            if (userWallEntryRepo != null)
            {
                userWallEntryRepo.Dispose();
            }

            if (wallEntryCacheRepo != null)
            {
                wallEntryCacheRepo.Dispose();
            }
        }
    }
}
