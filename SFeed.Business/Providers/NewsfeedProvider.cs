using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Models.Newsfeed;
using SFeed.RedisRepository.Implementation;
using System.Linq;
using SFeed.Core.Infrastructure.Caching;
using SFeed.Core.Models.Wall;
using SFeed.Core.Models.Caching;
using System;

namespace SFeed.Business.Providers
{
    public class NewsfeedProvider : INewsfeedProvider
    {
        INewsfeedCacheRepository feedCacheRepo;
        IFollowerProvider followerProvider;
        INewsfeedReaderCacheRepository newsFeedResponseRepo;

        public NewsfeedProvider() : this(
            new RedisNewsfeedEntryRepository(),
            new FollowerProvider(),
            new RedisNewsfeedResponseRepository())
        {

        }
        public NewsfeedProvider(
            INewsfeedCacheRepository feedCacheRepo,
            IFollowerProvider followerProvider,
            INewsfeedReaderCacheRepository newsFeedResponseRepo)
        {
            this.feedCacheRepo = feedCacheRepo;
            this.followerProvider = followerProvider;
            this.newsFeedResponseRepo = newsFeedResponseRepo;
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
                followers = followerProvider.GetUserFollowers(entryBy);

                if (string.Equals(entryBy, targetWall.OwnerId))
                {
                    //user posted to his/her own wall no action required
                }
                else if (targetWall.IsPublic)
                {
                    //target user profile is public, add target user followers
                    var targetUserFollowers = followerProvider.GetUserFollowers(entryBy);
                    followers = followers.Union(targetUserFollowers);
                }
            }
            //user posts to a group wall
            else
            {
                if (!targetWall.IsPublic)
                {
                    //For private group posts, only users that can follow target group gets newsfeed item
                    followers = followerProvider.GetGroupFollowers(targetWall.OwnerId);
                }
                else
                {
                    //for public groups notify both
                    followers = followerProvider.GetUserFollowers(entryBy);
                    var groupFollowers = followerProvider.GetGroupFollowers(targetWall.OwnerId);
                    followers = followers.Union(groupFollowers);
                }
            }
            return followers.Distinct();
        }

        public IEnumerable<NewsfeedResponseModel> GetUserNewsfeed(string userId, int skip, int take)
        {
            return newsFeedResponseRepo.GetUserFeed(userId, skip, take);
        }
    }
}
