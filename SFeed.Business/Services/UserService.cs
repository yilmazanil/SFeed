using System;
using System.Collections.Generic;
using SFeed.Model;
using SFeed.Data.Infrastructure;
using System.Linq;
using SFeed.Data.SqlRepositories;
using SFeed.Data;
using AutoMapper;

namespace SFeed.Business.Services
{
    public class UserService : IDisposable
    {
        private readonly ISqlRepository<User> userRepository;
        private readonly ISqlRepository<UserFollower> userFollowerRepository;


        public UserService(ISqlRepository<User> userRepo, ISqlRepository<UserFollower> followerRepo)
        {
            this.userRepository = userRepo;
            this.userFollowerRepository = followerRepo;
        }

        public UserService()
        {
            this.userRepository = new UserRepository();
            this.userFollowerRepository = new UserFollowerRepository();
        }

        public void AddUser(User user)
        {
            userRepository.Add(user);
            userRepository.Commit();
        }

        public IEnumerable<int> GetFollowers(int userId)
        {
           return userFollowerRepository.GetMany(p => p.UserId == userId).Select(u=>u.FollowerId);
        }

        public UserViewModel GetUser(string username)
        {
            var result =  userRepository.Get(u => u.Username == username);
            return Mapper.Map<UserViewModel>(result);
        }

        public void Dispose()
        {
            userRepository.Dispose();
            userFollowerRepository.Dispose();
        }
    }
}
