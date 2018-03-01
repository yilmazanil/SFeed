using SFeed.Core.Infrastructue.Repository;
using SFeed.Core.Infrastructue.Services;
using SFeed.Core.Models;
using SFeed.RedisRepository;
using System.Collections.Generic;
using System;

namespace SFeed.Business.Services
{
    public class UserFeedService : IUserNewsfeedService, IDisposable
    {
        ICacheListRepository<FeedItemModel> redisFeedRepo;
        ITypedCacheRepository<WallEntryModel> redisWallEntryRepo;
        //ICacheListRepository<FeedItemModel> redisGlobalFeedRepo;

        public UserFeedService()
        {
            this.redisFeedRepo = new RedisUserFeedRepository();
            this.redisWallEntryRepo = new RedisWallEntryRepository();
            //this.redisGlobalFeedRepo = new RedisGlobalFeedRepository();
        }

        public void AddToUserFeeds(FeedItemModel feedItem, IEnumerable<string> userIds)
        {
            foreach (var userId in userIds)
            {
                redisFeedRepo.AddToList(userId, feedItem);
            }
        }

        public void Dispose()
        {
            if (redisFeedRepo != null)
            {
                redisFeedRepo.Dispose();
            }
            if (redisWallEntryRepo != null)
            {
                redisWallEntryRepo.Dispose();
            }
        }

        public IEnumerable<FeedItemModel> GetUserFeed(string userId)
        {
           return redisFeedRepo.GetList(userId);
        }
    }
}
