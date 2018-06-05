using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Models.Newsfeed;
using SFeed.RedisRepository.Implementation;
using System.Linq;
using SFeed.Core.Infrastructure.Caching;
using SFeed.Core.Models.Wall;
using SFeed.Core.Models.Caching;
using System;
using SFeed.Core.Infrastructure.Repository;
using SFeed.SqlRepository.Implementation;

namespace SFeed.Business.Providers
{
    public class NewsfeedProvider : INewsfeedProvider
    {
        INewsfeedCacheRepository feedCacheRepo;
        INewsfeedRepository feedRepo;
        IFollowerProvider followerProvider;
        INewsfeedReaderCacheRepository newsFeedResponseRepo;

        public NewsfeedProvider() : this(
            new RedisNewsfeedEntryRepository(),
            new FollowerProvider(),
            new RedisNewsfeedResponseRepository(),
            new NewsfeedRepository())
        {

        }
        public NewsfeedProvider(
            INewsfeedCacheRepository feedCacheRepo,
            IFollowerProvider followerProvider,
            INewsfeedReaderCacheRepository newsFeedResponseRepo,
            INewsfeedRepository newsFeedRepo)
        {
            this.feedCacheRepo = feedCacheRepo;
            this.followerProvider = followerProvider;
            this.newsFeedResponseRepo = newsFeedResponseRepo;
            this.feedRepo = newsFeedRepo;
        }

        public void AddNewsfeedItem(NewsfeedItem newsFeedEntry)
        {
            var followers = GetFollowers(newsFeedEntry.By, newsFeedEntry.WallOwner);

            if (followers.Any())
            {
                var cacheModel = new NewsfeedEventModel
                {
                    By = newsFeedEntry.By,
                    ActionType = newsFeedEntry.FeedType,
                    ReferencePostId = newsFeedEntry.ReferencePostId
                };
                feedCacheRepo.AddEvent(cacheModel, followers);
            }
        }

        public void RemoveNewsfeedItem(NewsfeedItem newsFeedEntry)
        {
            if (newsFeedEntry.FeedType == NewsfeedActionType.wallpost)
            {
                RemovePost(newsFeedEntry);
            }
            else
            {
                var followers = GetFollowers(newsFeedEntry.By, newsFeedEntry.WallOwner);

                if (followers.Any())
                {
                    var cacheModel = new NewsfeedEventModel
                    {
                        By = newsFeedEntry.By,
                        ActionType = newsFeedEntry.FeedType,
                        ReferencePostId = newsFeedEntry.ReferencePostId
                    };
                    feedCacheRepo.RemoveEvent(cacheModel, followers);
                }
            }
        }


        public void RemovePost(NewsfeedItem newsFeedEntry)
        {
            var followers = GetFollowers(newsFeedEntry.By, newsFeedEntry.WallOwner);

            if (followers.Any())
            {
                var newsFeedCacheModel = new NewsfeedEventModel
                {
                    By = newsFeedEntry.By,
                    ActionType = newsFeedEntry.FeedType,
                    ReferencePostId = newsFeedEntry.ReferencePostId
                };
                feedCacheRepo.RemoveEvent(newsFeedCacheModel, followers);
            }
        }

        private IEnumerable<string> GetFollowers(string entryBy, NewsfeedWallModel targetWall)
        {
            var configProvider = new ApplicationConfigurationRepository();
            var parameters = configProvider.FetchConfiguration();

            bool displayOnOwnFeed = true;
            bool displayOnlyOnPrivateGroupFollowers = true;
            bool displayOnPrivateGroupFollowersFeed = true;
            bool displayOnPublicGroupFollowersFeed = true;
            bool displayOnUserFollowers = true;
            bool displayOnWallOwnerFollowersFeed = true;

            IEnumerable<string> followers = new List<string>();

            if (displayOnOwnFeed)
            {
                followers = followers.Union(new List<string> { entryBy });
            }

            if (targetWall.WallOwnerType == WallType.group && !targetWall.IsPublic && displayOnlyOnPrivateGroupFollowers)
            {
                var privateGroupFollowers = followerProvider.GetGroupFollowersCached(targetWall.OwnerId);
                followers = followers.Union(privateGroupFollowers);
            }
            else
            {
                if (targetWall.WallOwnerType == WallType.group
                    && ((!targetWall.IsPublic && displayOnPrivateGroupFollowersFeed)
                        || (targetWall.IsPublic && displayOnPublicGroupFollowersFeed)))
                {
                    var groupFollowers = followerProvider.GetGroupFollowersCached(targetWall.OwnerId);
                    followers = followers.Union(groupFollowers);
                }
                else if (displayOnWallOwnerFollowersFeed)
                {
                    var targetUserFollowers = followerProvider.GetUserFollowersCached(targetWall.OwnerId);
                    followers = followers.Union(targetUserFollowers);
                }
                if (displayOnUserFollowers)
                {
                    var userFollowers = followerProvider.GetUserFollowersCached(entryBy);
                    followers = followers.Union(userFollowers);
                }
            }
            return followers.Distinct();
        }

        public IEnumerable<NewsfeedResponseModel> GetUserNewsfeed(string userId, int skip, int take)
        {
            return newsFeedResponseRepo.GetUserFeed(userId, skip, take);
        }

        public void GenerateNewsfeed(string userId)
        {
            var feeds = feedRepo.Generate(userId);
            feedCacheRepo.UpdateFeed(userId, feeds);
        }
    }
}
