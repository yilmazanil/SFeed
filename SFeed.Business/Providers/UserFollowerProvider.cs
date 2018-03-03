using SFeed.Core.Infrastructue.Repository;
using SFeed.Core.Infrastructure.Providers;
using SFeed.RedisRepository;
using SFeed.SqlRepository;
using System.Collections.Generic;
using System.Linq;

namespace SFeed.Business.Providers
{
    public class UserFollowerProvider : IUserFollowerProvider
    {
        private readonly IRepository<UserFollower> followerRepo;
        private readonly ICacheListRepository<string> followerCacheRepo;

        public UserFollowerProvider(): this(
            new UserFollowerRepository(),
            new RedisUserFollowerRepository())
        {

        }

        public UserFollowerProvider(
            IRepository<UserFollower> followerRepo,
            ICacheListRepository<string> followerCacheRepo)
        {
            this.followerRepo = followerRepo;
            this.followerCacheRepo = followerCacheRepo;
        }
        public void FollowUser(string followerId, string userId)
        {
            bool alreadyFollowing = followerRepo.Any(p => p.FollowerId == followerId && p.UserId == userId);
            if (!alreadyFollowing)
            {
                followerRepo.Add(new UserFollower { FollowerId = followerId, UserId = userId });
                followerRepo.CommitChanges();
                followerCacheRepo.AppendToList(userId, followerId);
            }
        }

        public IEnumerable<string> GetFollowers(IEnumerable<string> userIds)
        {
            var userIdList = new List<string>();
            foreach (var userId in userIds)
            {
                userIdList.AddRange(GetFollowers(userId));
            }
            return userIdList.Distinct();
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

            return result.Distinct();
        }

        public void UnfollowUser(string followerId, string userId)
        {
            followerRepo.Delete(f => f.FollowerId == followerId && f.UserId == userId);
            followerRepo.CommitChanges();
            followerCacheRepo.RemoveFromList(userId, followerId);
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

    }
}
