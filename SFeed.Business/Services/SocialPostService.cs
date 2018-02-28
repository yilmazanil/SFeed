using AutoMapper;
using SFeed.Business.Infrastructure;
using SFeed.Data;
using SFeed.Data.Infrastructure;
using SFeed.Data.RedisRepositories;
using SFeed.Data.SqlRepositories;
using SFeed.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFeed.Business.Services
{
    public class SocialPostService : IDisposable
    {
        IUserWallService userWallService;
        IRedisTypedRepository<SocialPostModel> redisSocialPostRepo;
        IFollowerService followerService;
        IUserFeedService userFeedService;


        public SocialPostService()
        {
            this.userWallService = new UserWallService();
            this.redisSocialPostRepo = new RedisSocialPostRepository();
            this.followerService = new FollowerService();
            this.userFeedService = new UserFeedService();
        }

        public SocialPostService(IUserWallService userWallService,
            IRedisTypedRepository<SocialPostModel> redisSocialPostRepo, 
            IFollowerService followerService
            , IUserFeedService userFeedService
            )
        {
            this.userWallService = userWallService;
            this.redisSocialPostRepo = redisSocialPostRepo;
            this.followerService = followerService;
            this.userFeedService = userFeedService;
        }

        public SocialPostModel Create(SocialPostModel model, int targetUserId)
        {
            var dbEntry = new SocialPost { Body = model.Body, CreatedBy = model.CreatedBy, IsDeleted = false, CreatedDate = DateTime.Now };
            var userWallEntry = new UserWall { UserId = targetUserId };
            long postId = 0;

            try
            {
                postId = userWallService.PublishToUserWall(targetUserId, model);
                if (postId > 0)
                {
                    model.Id = postId;
                    redisSocialPostRepo.Add(model);
                }

                var followers = followerService.GetFollowers(model.CreatedBy);
                if (model.CreatedBy != targetUserId)
                {
                    var targetUserFollowers = followerService.GetFollowers(model.CreatedBy);
                    followers = Enumerable.Union<int>(followers, targetUserFollowers);
                }
                followers = Enumerable.Union<int>(followers, new List<int> { model.CreatedBy });
                userFeedService.AddToUserFeeds(postId, followers);

            }
            catch (Exception)
            {
                userWallService.Delete(postId);
                userFeedService.DeleteFromFeeds(postId);
            }
            return model;
        }

        public IEnumerable<SocialPostModel> GetUserFeed(int userId)
        {
            return userFeedService.GetUserFeed(userId);
        }

        public void Dispose()
        {
            
        }
    }
}
