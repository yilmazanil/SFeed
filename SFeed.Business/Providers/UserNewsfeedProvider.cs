using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;
using SFeed.Core.Models.Newsfeed;
using AutoMapper;

namespace SFeed.Business.Providers
{
    public class UserNewsfeedProvider : IUserNewsfeedProvider
    {
        ICacheListRepository<NewsfeedEntry> feedCacheRepo;
        ITypedCacheRepository<WallPostCacheModel> wallPostCacheRepo;
        IUserFollowerProvider userFollowerProvider;

        public UserNewsfeedProvider() : this(new RedisUserFeedRepository(), new RedisWallPostRepository(),
            new UserFollowerProvider())
        {

        }
        public UserNewsfeedProvider(ICacheListRepository<NewsfeedEntry> feedCacheRepo,
            ITypedCacheRepository<WallPostCacheModel> wallPostCacheRepo,
            IUserFollowerProvider userFollowerProvider)
        {
            this.feedCacheRepo = feedCacheRepo;
            this.wallPostCacheRepo = wallPostCacheRepo;
            this.userFollowerProvider = userFollowerProvider;
        }
         


        public void AddNewsfeedItem(NewsfeedEntry entry)
        {
            var followers = GetFollowers(entry);

            foreach (var userId in followers)
            {
                feedCacheRepo.GetList(userId);
                feedCacheRepo.PrependToList(userId, entry);
            }
        }

        public void DeleteNewsfeedItem(NewsfeedEntry entry)
        {
            var followers = GetFollowers(entry);

            foreach (var userId in followers)
            {
                feedCacheRepo.RemoveFromList(userId, entry);
            }
        }

        public IEnumerable<NewsfeedResponseItem> GetUserNewsfeed(string userId)
        {
            var feeds = feedCacheRepo.GetList(userId);

            foreach (var feed in feeds)
            {

                var item = Mapper.Map<NewsfeedResponseItem>(feed);
                if (!string.IsNullOrWhiteSpace(feed.ReferencePostId))
                {
                    var referencePost = wallPostCacheRepo.GetItem(feed.ReferencePostId);
                    if (item.TypeId == (short)  NewsfeedEntryType.wallpost && referencePost == null)
                    {
                        continue;
                    }
                    else
                    {
                        item.ReferencedPost = referencePost;
                    }
                }
                yield return item;
            }
        }

        private IEnumerable<string> GetFollowers(NewsfeedEntry entry)
        {
            var actors = new List<string> { entry.From.Id };
            if (entry.To != null)
            {
                actors.Add(entry.To.Id);
            }
            return userFollowerProvider.GetFollowers(actors);
        }

        public void Dispose()
        {
            if (feedCacheRepo != null)
            {
                feedCacheRepo.Dispose();
            }
            if (wallPostCacheRepo != null)
            {
                wallPostCacheRepo.Dispose();
            }
            if (userFollowerProvider != null)
            {
                userFollowerProvider.Dispose();
            }
        }
    }
}
