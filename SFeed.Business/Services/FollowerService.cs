using System;
using SFeed.Business.Infrastructure;
using SFeed.Data;
using SFeed.Data.Infrastructure;
using SFeed.Data.RedisRepositories;
using SFeed.Data.SqlRepositories;
using System.Linq;
using System.Collections.Generic;

namespace SFeed.Business.Services
{
    public class FollowerService : IFollowerService
    {
        private readonly ISqlRepository<UserFollower> sqlFollowerRepo;
        private readonly IRedisListRepository<int, int> redisFollowerRepo;

        public FollowerService()
        {
            sqlFollowerRepo = new UserFollowerRepository();
            redisFollowerRepo = new RedisUserFollowerRepository();
        }

        public FollowerService(ISqlRepository<UserFollower> userFollowerRepository, 
            IRedisListRepository<int, int> cachedFollowersRepo)
        {
            sqlFollowerRepo = userFollowerRepository;
            redisFollowerRepo = cachedFollowersRepo;
        }
   
        public void FollowUser(int activeUserId, int userToFollow)
        {
            //Check if user is already following
            var user = sqlFollowerRepo.Get(f => f.FollowerId == userToFollow && f.UserId == activeUserId);
            if (user == null)
            {
                sqlFollowerRepo.Add(new UserFollower { UserId = userToFollow, FollowerId = activeUserId });
            }
            if (!redisFollowerRepo.Exists(userToFollow, activeUserId))
            {
                redisFollowerRepo.Add(userToFollow, activeUserId);
            }
        }
        public void UnFollowUser(int activeUserId, int userToUnFollow)
        {
            sqlFollowerRepo.Delete(f => f.FollowerId == activeUserId && f.UserId == userToUnFollow);
            redisFollowerRepo.Remove(userToUnFollow, activeUserId);
        }

        public IEnumerable<int> GetFollowers(int userId)
        {
            //check redis
            var result = redisFollowerRepo.Retrieve(userId);
            if (!result.Any())
            {
                //check sql
                result = sqlFollowerRepo.GetMany(p => p.UserId == userId).Select(u => u.FollowerId);
                if (result.Any())
                {
                    //update redis if missing
                    redisFollowerRepo.Recreate(userId, result);
                }
            }

            return result;
        }
    }
}
