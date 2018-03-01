using System;
using System.Linq;
using System.Collections.Generic;
using SFeed.Core.Infrastructue.Services;
using SFeed.Core.Infrastructue.Repository;
using SFeed.SqlRepository;
using SFeed.RedisRepository;

namespace SFeed.Business.Services
{
    public class FollowerService : IUserFollowerService, IDisposable
    {
        private readonly IRepository<UserFollower> followerRepo;
        private readonly ICacheListRepository<string> followerCacheRepo;

        public FollowerService() :
            this(new UserFollowerRepository(),
            new RedisUserFollowerRepository())
        { }

        public FollowerService(
            IRepository<UserFollower> followersRepo,
            ICacheListRepository<string> followersCacheRepo)
        {
            followerRepo = followersRepo;
            followerCacheRepo = followersCacheRepo;
        }

        public void FollowUser(string activeUserId, string userIdToFollow)
        {
            bool alreadyFollowing = followerRepo.Any(p => p.FollowerId == activeUserId && p.UserId == userIdToFollow);
            if (!alreadyFollowing)
            {
                followerRepo.Add(new UserFollower { FollowerId = activeUserId, UserId = userIdToFollow });
                followerRepo.CommitChanges();
            }
            bool alreadyInCache = followerCacheRepo.ExistsInList(userIdToFollow, activeUserId);
            if (!alreadyInCache)
            {
                followerCacheRepo.AddToList(userIdToFollow, activeUserId);
            }

        }
        public void UnFollowUser(string activeUserId, string userIdToUnFollow)
        {
            followerRepo.Delete(f => f.FollowerId == activeUserId && f.UserId == userIdToUnFollow);
            followerRepo.CommitChanges();
            followerCacheRepo.RemoveFromList(userIdToUnFollow, activeUserId);
        }

        public void Dispose()
        {
            if (followerRepo != null)
            {
                followerRepo.Dispose();
            }
            if (followerCacheRepo != null)
            {
                followerCacheRepo.Dispose();
            }
        }

        public IEnumerable<string> GetFollowers(string userId)
        {
            var result = followerCacheRepo.GetList(userId);
            if (!result.Any())
            {
                //There are no followers in cache
                result = followerRepo.GetMany(p => p.UserId == userId).Select(u => u.FollowerId);
                if (result.Any())
                {
                    //There are records in db, refresh cache
                    followerCacheRepo.RecreateList(userId, result);
                }
            }

            return result;
        }
    }
}
