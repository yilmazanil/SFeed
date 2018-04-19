using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Models.Newsfeed;
using SFeed.RedisRepository.Implementation;
using System.Linq;
using SFeed.Core.Models.Caching;
using SFeed.Core.Infrastructure.Caching;
using SFeed.Core.Models.Wall;

namespace SFeed.Business.Providers
{
    //TODO: Include wallpost owner followers for wallpost
    public class UserNewsfeedProvider : INewsfeedProvider
    {
        INewsfeedCacheRepository feedCacheRepo;
        IFollowerProvider followerProvider;

        public UserNewsfeedProvider() : this(
            new RedisNewsfeedEntryRepository(),
            new FollowerProvider())
        {

        }
        public UserNewsfeedProvider(
            INewsfeedCacheRepository feedCacheRepo,
            IFollowerProvider followerProvider)
        {
            this.feedCacheRepo = feedCacheRepo;
            this.followerProvider = followerProvider;
        }

        //public IEnumerable<NewsfeedResponseItem> GetUserNewsfeed(string userId)
        //{
        //    throw new NotImplementedException();
        //    return newsFeedResponseProvider.GetUserNewsfeed(userId);
        //}

        public void AddNewsfeedItem(NewsfeedItem newsFeedEntry)
        {
            var followers = GetFollowers(newsFeedEntry.By, newsFeedEntry.WallOwner);

            if (followers.Any())
            {
                var cacheModel = new NewsfeedEventModel
                {
                    By = newsFeedEntry.By,
                    FeedType = newsFeedEntry.FeedType,
                    ReferencePostId = newsFeedEntry.ReferencePostId
                };
                feedCacheRepo.AddEvent(cacheModel, followers);
            }
        }

        //public IEnumerable<NewsfeedWallPostModel> GetUserFeed(string userId, int skip, int take)
        //{
        //    return feedCacheRepo.GetUserFeed(userId, skip, take);
        //}

        public void RemoveNewsfeedItem(NewsfeedItem newsFeedEntry)
        {
            if (newsFeedEntry.FeedType == NewsfeedEventType.wallpost)
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
                        FeedType = newsFeedEntry.FeedType,
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
                    FeedType = newsFeedEntry.FeedType,
                    ReferencePostId = newsFeedEntry.ReferencePostId
                };
                feedCacheRepo.RemoveEvent(newsFeedCacheModel, followers);
            }
        }

        //public void RemoveFeedsFromUser(string fromUser, string byUser)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RemoveFeedsFromGroup(string fromUser, string byGroup)
        //{
        //    throw new NotImplementedException();
        //}


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
    }
}
