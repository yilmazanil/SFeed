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

        public UserFeedService()
        {
            this.redisFeedRepo = new RedisFeedRepository();
            this.redisWallEntryRepo = new RedisWallEntryRepository();
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

        public IEnumerable<WallEntryModel> GetUserFeed(string userId)
        {
            var feedRef = redisFeedRepo.GetList(userId);

            foreach (var item in feedRef)
            {
                if (item.EntryType == (short)FeedEntryTypeEnum.WallEntry)
                {
                    yield return redisWallEntryRepo.GetItem(item.ReferenceId);
                }
            }
        }
    }
}
