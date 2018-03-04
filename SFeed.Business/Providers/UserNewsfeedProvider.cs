using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models.Caching;
using System;

namespace SFeed.Business.Providers
{
    public class UserNewsfeedProvider : IUserNewsfeedProvider
    {
        ICacheListRepository<NewsfeedEntry> feedCacheRepo;
        ITypedCacheRepository<WallPostNewsfeedModel> wallPostCacheRepo;

        public UserNewsfeedProvider() : this(new RedisUserFeedRepository(), new RedisWallPostRepository())
        {

        }
        public UserNewsfeedProvider(ICacheListRepository<NewsfeedEntry> feedCacheRepo,
            ITypedCacheRepository<WallPostNewsfeedModel> wallPostCacheRepo)
        {
            this.feedCacheRepo = feedCacheRepo;
            this.wallPostCacheRepo = wallPostCacheRepo;

        }

        public IEnumerable<NewsfeedResponseItem> GetUserFeed(string userId)
        {
            var feeds =  feedCacheRepo.GetList(userId);

            foreach (var feed in feeds)
            {
                if (feed.TypeId == (short)NewsfeedEntryType.wallpost)
                {
                    yield return new NewsfeedResponseItem
                    {

                        Item = wallPostCacheRepo.GetItem(feed.ReferenceEntryId),
                        ItemId = feed.ReferenceEntryId,
                        ItemType = NewsfeedEntryType.wallpost

                    };
                }
            }
        }

        public void Dispose()
        {
            if (feedCacheRepo != null)
            {
                feedCacheRepo.Dispose();
            }
        }

        public void RemoveFromUsers(NewsfeedEntry item, IEnumerable<string> userIds)
        {
            foreach (var userId in userIds)
            {
                feedCacheRepo.RemoveFromList(userId, item);
            }
        }

        public void AddToUserFeeds<T>(T feedItem, NewsfeedEntryType entryType, IEnumerable<string> userIds) where T : TypedCacheItemBaseModel
        {
            var entryModel = new NewsfeedEntry { TypeId = (short)entryType, ReferenceEntryId = feedItem.Id };

            switch (entryType)
            {
                case NewsfeedEntryType.wallpost:
                    wallPostCacheRepo.AddItem(feedItem as WallPostNewsfeedModel);
                    break;
                default:
                    break;
            }

            foreach (var userId in userIds)
            {
                feedCacheRepo.PrependToList(userId, entryModel);
            }
        }

        public void UpdateFeed<T>(T feedItem, NewsfeedEntryType entryType) where T : TypedCacheItemBaseModel
        {
            switch (entryType)
            {
                case NewsfeedEntryType.wallpost:
                    wallPostCacheRepo.UpdateItem(feedItem.Id, feedItem as WallPostNewsfeedModel);
                    break;
                default:
                    break;
            }
        }

        public void Delete(NewsfeedEntry item)
        {
            var itemType = (NewsfeedEntryType)item.TypeId;
            switch (itemType)
            {
                case NewsfeedEntryType.wallpost:
                    wallPostCacheRepo.RemoveItem(item.ReferenceEntryId);
                    break;
                default:
                    break;
            }
        }
    }
}
