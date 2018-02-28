using System;
using System.Collections.Generic;
using SFeed.Model;
using SFeed.Data.Infrastructure;
using System.Linq;
using SFeed.Data.SqlRepositories;
using SFeed.Data;
using AutoMapper;
using SFeed.Data.RedisRepositories;

namespace SFeed.Business.Services
{
    public class UserService : IDisposable
    {
        private readonly ISqlRepository<User> userRepository;
        private readonly ISqlRepository<UserFollower> userFollowerRepository;
        private readonly IRedisListRepository<int, int> cachedFollowersRepo;


        public UserService(
            ISqlRepository<User> userRepo,
            ISqlRepository<UserFollower> followerRepo,
            IRedisListRepository<int, int> cachedFollowersRepo)
        {
            this.userRepository = userRepo;
            this.userFollowerRepository = followerRepo;
            this.cachedFollowersRepo = cachedFollowersRepo;
        }

        public UserService()
        {
            this.userRepository = new UserRepository();
            this.userFollowerRepository = new UserFollowerRepository();
            this.cachedFollowersRepo = new RedisUserFollowerRepository();
        }

        public int AddUser(UserModel user)
        {
            var newUser = new User { Username = user.Username };
            userRepository.Add(newUser);
            userRepository.Commit();
            return newUser.Id;
        }

        public IEnumerable<int> GetFollowers(int userId)
        {
            var result = cachedFollowersRepo.Retrieve(userId);
            if (!result.Any())
                 result =  userFollowerRepository.GetMany(p => p.UserId == userId).Select(u=>u.FollowerId);

            return result;
           
        }

        public UserModel GetUser(string username)
        {
            var result =  userRepository.Get(u => u.Username == username);
            return Mapper.Map<UserModel>(result);
        }

        public void FollowUser(int activeUserId, int userId)
        {
            var followers = GetFollowers(userId);
            if (followers.Any() && followers.Contains(activeUserId))
            {
                throw new Exception("Cannot follow user twice");
            }
            userFollowerRepository.Add(new UserFollower { UserId = userId, FollowerId = activeUserId });
            userFollowerRepository.Commit();
            cachedFollowersRepo.Add(userId, activeUserId);
        }
        public void UnFollowUser(int activeUserId, int userId)
        {
            userFollowerRepository.Delete(p => p.FollowerId == activeUserId && p.UserId == userId);
            userFollowerRepository.Commit();
            cachedFollowersRepo.Remove(userId, activeUserId);
        }
        public void Dispose()
        {
            userRepository.Dispose();
            userFollowerRepository.Dispose();
        }
    }
}
