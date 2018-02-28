using SFeed.Business.Infrastructure;
using System.Collections.Generic;
using SFeed.Model;
using SFeed.Data.Infrastructure;
using SFeed.Data.RedisRepositories;
using System;
using SFeed.Data;
using System.Linq;
using SFeed.Data.SqlRepositories;

namespace SFeed.Business.Services
{
    public class UserFeedService : IUserFeedService
    {
        IRedisUserFeedRepository redisFeedRepo;
        ISqlRepository<User> userSqlRepo;

        public UserFeedService()
        {
            this.redisFeedRepo = new RedisFeedRepository();
            this.userSqlRepo = new UserRepository();
        }

        public UserFeedService(IRedisUserFeedRepository redisFeedRepo,
             ISqlRepository<User> userSqlRepo)
        {
            this.redisFeedRepo = redisFeedRepo;
            this.userSqlRepo = userSqlRepo;
        }
        public void AddToUserFeeds(long postId, IEnumerable<int> users)
        {
            redisFeedRepo.AddToUserFeeds(users, postId);
        }

        public void DeleteFromFeeds(long postId)
        {
            //TODO: Refactor according to datastructure update
            var users = userSqlRepo.GetAll().Select(u => u.Id);
            redisFeedRepo.DeleteFromUserFeeds(postId, users);
        }

        public IEnumerable<SocialPostModel> GetUserFeed(int userId)
        {
            return redisFeedRepo.GetUserFeeds(userId);
        }
    }
}
