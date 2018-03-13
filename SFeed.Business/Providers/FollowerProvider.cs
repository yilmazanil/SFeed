using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.SqlRepository.Implementation;
using SFeed.RedisRepository.Implementation;
using SFeed.Core.Infrastructure.Caching;
using SFeed.Core.Infrastructure.Repository;

namespace SFeed.Business.Providers
{
    public class FollowerProvider : IFollowerProvider
    {
        IFollowerCacheRepository followerCacheRepo;
        IFollowerRepository followerRepo;

        public FollowerProvider() : this(
            new RedisFollowerRepository(),
            new FollowerRepository()
            )
        {

        }

        public FollowerProvider(
            IFollowerCacheRepository followerCacheRepo,
            IFollowerRepository followerRepo)
        {
            this.followerCacheRepo = followerCacheRepo;
            this.followerRepo = followerRepo;
        }

        public void FollowUser(string followerId, string userId)
        {
            followerRepo.FollowUser(userId, followerId);
            followerCacheRepo.FollowUser(userId, followerId);
        }

        public void UnfollowUser(string followerId, string userId)
        {
            followerRepo.UnfollowUser(userId, followerId);
            followerCacheRepo.UnfollowUser(userId, followerId);
        }

        public void FollowGroup(string followerId, string groupId)
        {
            followerRepo.FollowGroup(groupId, followerId);
            followerCacheRepo.FollowGroup(groupId, followerId);
        }

        public void UnfollowGroup(string followerId, string groupId)
        {
            followerRepo.UnfollowGroup(groupId, followerId);
            followerCacheRepo.UnfollowGroup(groupId, followerId);
        }

        public IEnumerable<string> GetUserFollowers(string userId)
        {
            return followerRepo.GetUserFollowers(userId);
        }

        public IEnumerable<string> GetGroupFollowers(string groupId)
        {
            return followerRepo.GetGroupFollowers(groupId);
        }


    }
}
