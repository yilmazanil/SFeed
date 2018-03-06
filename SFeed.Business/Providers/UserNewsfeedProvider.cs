﻿using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;
using SFeed.Core.Models.Newsfeed;
using AutoMapper;
using SFeed.Core.Models;

namespace SFeed.Business.Providers
{
    public sealed class UserNewsfeedProvider : IUserNewsfeedProvider
    {
        ICacheListRepository<NewsfeedEntry> feedCacheRepo;
        IWallPostCacheManager wallPostCacheManager;
        IFollowerProvider userFollowerProvider;

        public UserNewsfeedProvider() : this(
            new RedisUserFeedRepository(), 
            new WallPostCacheManager(),
            new FollowerProvider())
        {

        }
        public UserNewsfeedProvider(ICacheListRepository<NewsfeedEntry> feedCacheRepo,
            IWallPostCacheManager wallPostCacheManager,
            IFollowerProvider userFollowerProvider)
        {
            this.feedCacheRepo = feedCacheRepo;
            this.wallPostCacheManager = wallPostCacheManager;
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
                    var referencePost = wallPostCacheManager.GetPost(feed.ReferencePostId);
                    if (item.TypeId == (short)NewsfeedEntryType.wallpost && referencePost == null)
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
            var actors = new List<Actor> { new Actor { Id = entry.From.Id, ActorTypeId = (short)ActorType.user } };
            if (entry.To != null)
            {
                actors.Add(new Actor { Id = entry.To.Id, ActorTypeId = (short)ActorType.user });
            }
            return userFollowerProvider.GetFollowers(actors);
        }

        public void Dispose()
        {
            if (feedCacheRepo != null)
            {
                feedCacheRepo.Dispose();
            }
            if (wallPostCacheManager != null)
            {
                wallPostCacheManager.Dispose();
            }
            if (userFollowerProvider != null)
            {
                userFollowerProvider.Dispose();
            }
        }
    }
}
