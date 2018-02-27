using SFeed.Business.Caching;
using SFeed.Data;
using SFeed.Data.Infrastructure;
using SFeed.Data.RedisRepositories;
using SFeed.Data.SqlRepositories;
using SFeed.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SFeed.Business.Services
{
    public class SocialPostService : IDisposable
    {
        private readonly ISqlRepository<SocialPost> socialPostRepository;
        private readonly ISqlRepository<UserWall> userWallRepository;

        public SocialPostService(ISqlRepository<SocialPost> postRepository, ISqlRepository<UserWall> userWallRepository)
        {
            this.socialPostRepository = postRepository;
            this.userWallRepository = userWallRepository;
        }

        public SocialPostService()
        {
            this.socialPostRepository = new SocialPostRepository();
            this.userWallRepository = new UserWallRepository();
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

                SocialPostCacheRepository cacheRepo = new SocialPostCacheRepository();
                cacheRepo.Add(dbEntry);

                var userFollowerCache = new UserFollowerCacheRepository();
                var followers = userFollowerCache.Retrieve(model.CreatedBy);
                if (model.CreatedBy != targetUserId)
                {
                    var targetUserFollowers = userFollowerCache.Retrieve(targetUserId);
                    followers = Enumerable.Union<int>(followers, targetUserFollowers);
                }
                UserFeedRedisCache.AddFeed(followers, model.Id);

            }
            catch (Exception)
            {
                userWallRepository.Delete(userWallEntry);
                socialPostRepository.Delete(dbEntry);
            }
            return dbEntry.Id;
        }

        public IEnumerable<SocialPostViewModel> GetUserFeed(int userId)
        {
            return UserFeedRedisCache.GetFeed(userId);
        }
        public void Dispose()
        {
            socialPostRepository.Dispose();
            userWallRepository.Dispose();
        }
    }
}
