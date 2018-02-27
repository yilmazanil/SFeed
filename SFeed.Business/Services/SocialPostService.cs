using AutoMapper;
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
        private readonly ISqlRepository<SocialPost> socialPostRepository;
        private readonly ISqlRepository<UserWall> userWallRepository;
        private readonly IRedisTypedRepository<SocialPost> cachedPostRepo;
        private readonly IRedisListRepository<int, int> cachedFollowersRepo;
        private readonly IRedisUserFeedRepository cachedFeedRepo;



        public SocialPostService(ISqlRepository<SocialPost> postRepository,
            ISqlRepository<UserWall> userWallRepository,
            IRedisTypedRepository<SocialPost> cachedPostRepo,
            IRedisListRepository<int, int> cachedFollowersRepo,
            IRedisUserFeedRepository cachedFeedRepo
            )
        {
            this.socialPostRepository = postRepository;
            this.userWallRepository = userWallRepository;
            this.cachedPostRepo = cachedPostRepo;
            this.cachedFollowersRepo = cachedFollowersRepo;
            this.cachedFeedRepo = cachedFeedRepo;
        }

        public SocialPostService()
        {
            this.socialPostRepository = new SocialPostRepository();
            this.userWallRepository = new UserWallRepository();
            this.cachedPostRepo = new RedisSocialPostRepository();
            this.cachedFollowersRepo = new RedisUserFollowerRepository();
            this.cachedFeedRepo = new RedisFeedRepository();
        }

        public long Create(SocialPostViewModel model, int targetUserId)
        {
            var dbEntry = new SocialPost { Body = model.Body, CreatedBy = model.CreatedBy, IsDeleted = false, CreatedDate = DateTime.Now };
            var userWallEntry = new UserWall { UserId = targetUserId };

            try
            {
                socialPostRepository.Add(dbEntry);
                socialPostRepository.Commit();

                model.Id = dbEntry.Id;
                userWallEntry.SocialPostId = dbEntry.Id;
                userWallRepository.Add(userWallEntry);
                userWallRepository.Commit();


                cachedPostRepo.Add(dbEntry);


                var followers = cachedFollowersRepo.Retrieve(model.CreatedBy);
                if (model.CreatedBy != targetUserId)
                {
                    var targetUserFollowers = cachedFollowersRepo.Retrieve(targetUserId);
                    followers = Enumerable.Union<int>(followers, targetUserFollowers);
                }
                cachedFeedRepo.AddToUserFeeds(followers, model.Id);

            }
            catch (Exception)
            {
                userWallRepository.Delete(userWallEntry);
                socialPostRepository.Delete(dbEntry);
            }
            return dbEntry.Id;
        }

        public IEnumerable<SocialPost> GetUserFeed(int userId)
        {
            return cachedFeedRepo.GetUserFeeds(userId);
        }
  
        public void Dispose()
        {
            socialPostRepository.Dispose();
            userWallRepository.Dispose();
        }
    }
}
