using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models.Caching;
using System;
using SFeed.Core.Models;
using SFeed.Core;
using AutoMapper;

namespace SFeed.Business.Providers
{
    public class UserNewsfeedProvider : IUserNewsfeedProvider
    {
        ICacheListRepository<NewsfeedAction> feedCacheRepo;
        ITypedCacheRepository<WallPostNewsfeedModel> wallPostCacheRepo;
        IUserFollowerProvider userFollowerProvider;

        public UserNewsfeedProvider() : this(new RedisUserFeedRepository(), new RedisWallPostRepository(),
            new UserFollowerProvider())
        {

        }
        public UserNewsfeedProvider(ICacheListRepository<NewsfeedAction> feedCacheRepo,
            ITypedCacheRepository<WallPostNewsfeedModel> wallPostCacheRepo,
            IUserFollowerProvider userFollowerProvider)
        {
            this.feedCacheRepo = feedCacheRepo;
            this.wallPostCacheRepo = wallPostCacheRepo;
            this.userFollowerProvider = userFollowerProvider;

        }
         
        public void AddPost(WallPostNewsfeedModel wallPost)
        {
            var newsFeedEntry = new NewsfeedAction
            {
                ActionId = (short)NewsfeedActionType.wallpost,
                From = wallPost.PostedBy,
                To = new Actor { ActorTypeId = (short)ActorType.user, Id = wallPost.PostedBy },
                ReferencePostId = wallPost.Id
            };

            wallPostCacheRepo.AddItem(wallPost);

            var followers = userFollowerProvider.GetFollowers(new List<string> { wallPost.PostedBy, wallPost.WallOwnerId });

            foreach (var userId in followers)
            {
                feedCacheRepo.PrependToList(userId, newsFeedEntry);
            }

        }

        public void UpdatePost(WallPostNewsfeedModel wallPost)
        {
            wallPostCacheRepo.UpdateItem(wallPost.Id, wallPost);
        }

        public void DeletePost(string Id)
        {
            wallPostCacheRepo.RemoveItem(Id);
        }


        public void AddAction(NewsfeedAction newsFeedAction)
        {
            var actors = new List<string> { newsFeedAction.From };
            if (newsFeedAction.To != null)
            {
                actors.Add(newsFeedAction.To.Id);
            }
            var followers = userFollowerProvider.GetFollowers(actors);

            foreach (var userId in followers)
            {
                feedCacheRepo.PrependToList(userId, newsFeedAction);
            }

        }

        public void RemoveAction(NewsfeedAction newsFeedAction)
        {
            var actors = new List<string> { newsFeedAction.From };
            if (newsFeedAction.To != null)
            {
                actors.Add(newsFeedAction.To.Id);
            }
            var followers = userFollowerProvider.GetFollowers(actors);

            foreach (var userId in followers)
            {
                feedCacheRepo.PrependToList(userId, newsFeedAction);
            }
        }

        public void DeleteAction(NewsfeedAction newsFeedAction)
        {
            var actors = new List<string> { newsFeedAction.From };
            if (newsFeedAction.To != null)
            {
                actors.Add(newsFeedAction.To.Id);
            }
            var followers = userFollowerProvider.GetFollowers(actors);

            foreach (var userId in followers)
            {
                feedCacheRepo.RemoveFromList(userId, newsFeedAction);
            }
        }

        public IEnumerable<NewsfeedResponseItem> GetNewsfeed(string userId)
        {
            var feeds = feedCacheRepo.GetList(userId);

            foreach (var feed in feeds)
            {

                var item = Mapper.Map<NewsfeedResponseItem>(feed);
                if (!string.IsNullOrWhiteSpace(feed.ReferencePostId))
                {
                    var referencePost = wallPostCacheRepo.GetItem(feed.ReferencePostId);
                    if (item.ActionId == (short)  NewsfeedActionType.wallpost && referencePost == null)
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
