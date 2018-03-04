using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;
using SFeed.Core.Models.Newsfeed;

namespace SFeed.Business.Providers
{
    public class UserNewsfeedProvider : IUserNewsfeedProvider
    {
        ICacheListRepository<NewsfeedEntry> feedCacheRepo;
        ITypedCacheRepository<NewsfeedWallPostModel> wallPostCacheRepo;

        public UserNewsfeedProvider() : this(new RedisUserFeedRepository(), new RedisWallPostRepository())
        {

        }
        public UserNewsfeedProvider(ICacheListRepository<NewsfeedEntry> feedCacheRepo,
            ITypedCacheRepository<NewsfeedWallPostModel> wallPostCacheRepo)
        {
            this.feedCacheRepo = feedCacheRepo;
            this.wallPostCacheRepo = wallPostCacheRepo;

        }
        public void AddToUserFeeds(NewsfeedWallPostModel feedItem, IEnumerable<string> userIds)
        {

            var entryModel = new NewsfeedEntry { TypeId = (short)NewsfeedEntryType.wallpost, ReferenceEntryId = feedItem.Id };

            wallPostCacheRepo.AddItem(feedItem);

            foreach (var userId in userIds)
            {
                feedCacheRepo.PrependToList(userId, entryModel);
            }
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

        public void RemoveFromFeed(NewsfeedEntry item, IEnumerable<string> userIds)
        {
            foreach (var userId in userIds)
            {
                feedCacheRepo.RemoveFromList(userId, item);
            }
        }

        public void UpdateFeed(NewsfeedWallPostModel feedItem)
        {
            wallPostCacheRepo.UpdateItem(feedItem.Id, feedItem);
        }
    }
}
