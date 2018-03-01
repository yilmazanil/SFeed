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
    public class UserWallService : IUserWallService, IDisposable
    {
        IRepository<WallEntry> wallEntryRepo;
        IRepository<UserWall> userWallRepo;
        ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo;

        public UserWallService(IRepository<WallEntry> wallEntryRepo,
            IRepository<UserWall> userWallRepo,
            ITypedCacheRepository<WallEntryModel> wallEntryCacheRepo)
        {
            this.wallEntryRepo = wallEntryRepo;
            this.userWallRepo = userWallRepo;
            this.wallEntryCacheRepo = wallEntryCacheRepo;
        }

        public UserWallService()
        {
            this.wallEntryRepo = new WallEntryRepository();
            this.userWallRepo = new UserWallRepository();
            this.wallEntryCacheRepo = new RedisWallEntryRepository();
        }
        public void Delete(Guid postId)
        {
            userWallRepo.Delete(p => p.WallEntryId == postId);
            userWallRepo.CommitChanges();
            wallEntryRepo.Delete(p => p.Id == postId);
            wallEntryRepo.CommitChanges();
        }

        public IEnumerable<WallEntryModel> GetUserWall(string userId)
        {
            //TODO:Use SP & Introduce new interface
            var postIds = userWallRepo.GetMany(p => p.UserId == userId).Select(p=>p.WallEntryId);
            var results=  wallEntryRepo.GetMany(p => postIds.Contains(p.Id));
            return Mapper.Map<IEnumerable<WallEntryModel>>(results);
        }

        public Guid PublishEntryToUserWall(WallEntryModel entry, string wallOwnerUserId)
        {
            var dbEntry = new WallEntry { Body = entry.Body, CreatedBy = entry.CreatedBy, IsDeleted = false, CreatedDate = DateTime.Now };
            var userWallEntry = new UserWall { UserId = wallOwnerUserId };
            Guid postId = Guid.Empty;

            try
            {
                wallEntryRepo.Add(dbEntry);
                wallEntryRepo.CommitChanges();

                if (dbEntry.Id != Guid.Empty)
                {
                    userWallRepo.Add(userWallEntry);
                    userWallRepo.CommitChanges();

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
                    userWallRepo.Delete(p => p.WallEntryId == postId);
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

            if (userWallRepo != null)
            {
                userWallRepo.Dispose();
            }

            if (wallEntryCacheRepo != null)
            {
                wallEntryCacheRepo.Dispose();
            }
        }
    }
}
