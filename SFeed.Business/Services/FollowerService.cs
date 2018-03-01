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

        public string ActiveUserId { get; private set; }

        public FollowerService(string activeUserId) : 
            this(activeUserId, new UserFollowerRepository(),
            new RedisUserFollowerRepository()){}

        public FollowerService(string activeUserId,
            IRepository<UserFollower> followersRepo,
            ICacheListRepository<string> followersCacheRepo)
        {
            ActiveUserId = activeUserId;
            followerRepo = followersRepo;
            followerCacheRepo = followersCacheRepo;
        }

        public void FollowUser(string userIdToFollow)
        {
            //Check if user is already following
            bool alreadyFollowing = followerRepo.Any(p => p.FollowerId == ActiveUserId && p.UserId == userIdToFollow);
            if (!alreadyFollowing)
            {
                followerRepo.Add(new UserFollower { FollowerId = ActiveUserId, UserId = userIdToFollow });
                followerRepo.CommitChanges();
            }
            bool alreadyInCache = followerCacheRepo.ExistsInList(userIdToFollow, ActiveUserId);
            if (!alreadyInCache)
            {
                followerCacheRepo.AddToList(userIdToFollow, ActiveUserId);
            }

        }
        public void UnFollowUser(string userIdToUnFollow)
        {
            followerRepo.Delete(f => f.FollowerId == ActiveUserId && f.UserId == userIdToUnFollow);
            followerRepo.CommitChanges();
            followerCacheRepo.RemoveFromList(userIdToUnFollow, ActiveUserId);
        }

        public IEnumerable<string> GetFollowers()
        {
            return GetFollowers(ActiveUserId);
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
