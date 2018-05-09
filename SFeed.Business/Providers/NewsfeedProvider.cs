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
            IEnumerable<string> followers;
            //user posts to another user wall
            if (targetWall.WallOwnerType == WallType.user)
            {
                followers = followerProvider.GetUserFollowersCached(entryBy);

                if (string.Equals(entryBy, targetWall.OwnerId))
                {
                    //user posted to his/her own wall no action required
                }
                else if (targetWall.IsPublic)
                {
                    //target user profile is public, add target user followers
                    var targetUserFollowers = followerProvider.GetUserFollowersCached(targetWall.OwnerId);
                    followers = followers.Union(targetUserFollowers);
                }
            }
            //user posts to a group wall
            else
            {
                if (!targetWall.IsPublic)
                {
                    //For private group posts, only users that can follow target group gets newsfeed item
                    followers = followerProvider.GetGroupFollowersCached(targetWall.OwnerId);
                }
                else
                {
                    //for public groups notify both
                    followers = followerProvider.GetUserFollowers(entryBy);
                    var groupFollowers = followerProvider.GetGroupFollowersCached(targetWall.OwnerId);
                    followers = followers.Union(groupFollowers);
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
