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
    //public class UserWallService : IUserWallService, IDisposable
    //{
    //    IRepository<WallEntry> wallEntryRepo;
    //    IRepository<UserWall> userWallRepo;

    //    public UserWallService(IRepository<WallEntry> wallEntryRepo,
    //        IRepository<UserWall> userWallRepo)
    //    {
    //        this.wallEntryRepo = wallEntryRepo;
    //        this.userWallRepo = userWallRepo;
    //    }

    //    public UserWallService()
    //    {
    //        this.wallEntryRepo = new WallEntryRepository();
    //        this.userWallRepo = new UserWallRepository();
    //    }

    //    public IEnumerable<WallEntryModel> GetUserWall(string userId)
    //    {
    //        //TODO:Use SP & Introduce new interface
    //        var postIds = userWallRepo.GetMany(p => p.UserId == userId).Select(p=>p.WallEntryId);
    //        var results = wallEntryRepo.GetMany(p => postIds.Contains(p.Id));
    //        return Mapper.Map<IEnumerable<WallEntryModel>>(results);
    //    }

    //    public void AddEntryToUserWall(string entryId, string wallOwnerUserId)
    //    {
    //        var entryIdGuid = Guid.Parse(entryId);
    //        var userWallEntry = new UserWall { UserId = wallOwnerUserId, WallEntryId = entryIdGuid };

    //        try
    //        {
    //            userWallRepo.Add(userWallEntry);
    //            userWallRepo.CommitChanges();

    //        }
    //        catch (Exception)
    //        {
    //            userWallRepo.Delete(p => p.WallEntryId == entryIdGuid);
    //            userWallRepo.CommitChanges();
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        if (wallEntryRepo != null)
    //        {
    //            wallEntryRepo.Dispose();
    //        }

    //        if (userWallRepo != null)
    //        {
    //            userWallRepo.Dispose();
    //        }

    //    }
    //}
}
