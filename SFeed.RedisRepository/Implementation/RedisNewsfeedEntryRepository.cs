using SFeed.RedisRepository.Base;
using System.Collections.Generic;
using System;
using SFeed.Core.Infrastructure.Caching;
using SFeed.Core.Models.Newsfeed;
using System.Linq;
using StackExchange.Redis;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisNewsfeedEntryRepository : RedisRepositoryBase, INewsfeedCacheRepository
    {
        public string FeedPrefix => RedisNameConstants.FeedRepoPrefix;

        public void AddEvent(NewsfeedEventModel entry, IEnumerable<string> followers)
        {
            var activityEntry = MapToActivity(entry);
            var keys = MapRedisKeys(entry.ReferencePostId, followers);

            var transaction = StackExchangeRedisConnectionProvider.GetTransaction();
            foreach (var key in keys)
            {
                transaction.SetAddAsync(key.PostFeedKey, entry.ReferencePostId);
                transaction.SortedSetIncrementAsync(key.PostActionsKey, activityEntry, 1);
            }
            transaction.Execute();
        }

        public void RemoveEvent(NewsfeedEventModel entry, IEnumerable<string> followers)
        {
            var activityEntry = MapToActivity(entry);
            var keys = MapRedisKeys(entry.ReferencePostId, followers);
            var isPostRemoval = entry.ActionType == NewsfeedActionType.wallpost;


            var transaction = StackExchangeRedisConnectionProvider.GetTransaction();
            if (isPostRemoval)
            {
                foreach (var key in keys)
                {
                    transaction.SetRemoveAsync(key.PostFeedKey, entry.ReferencePostId);
                    transaction.KeyDeleteAsync(key.PostActionsKey);
                }
            }
            else
            {
                foreach (var key in keys)
                {
                    transaction.SortedSetDecrementAsync(key.PostActionsKey, activityEntry, 1);
                    transaction.SortedSetRemoveRangeByScoreAsync(key.PostActionsKey,-10,0);
                }
            }
            transaction.Execute();
        }

        public void UpdateFeed(string userId, IEnumerable<NewsfeedEventModel> events)
        {
            var feedKey = GetEntryKey(FeedPrefix, userId);
            var server = StackExchangeRedisConnectionProvider.GetServer();
            var existingFeedKeys = server.Keys(pattern: feedKey).Select(p=>(RedisKey)p).ToArray();

            var transaction = StackExchangeRedisConnectionProvider.GetTransaction();
            transaction.KeyDeleteAsync(existingFeedKeys);

            foreach (var userEvent in events)
            {
                var activityEntry = MapToActivity(userEvent);
                var actionKey = GetEntryKey(FeedPrefix, string.Concat(userId, ":", userEvent.ReferencePostId));
                transaction.SetAddAsync(feedKey, userEvent.ReferencePostId);
                transaction.SortedSetIncrementAsync(actionKey, activityEntry, 1);
            }
            transaction.Execute();

        }
        private IEnumerable<FollowerRedisKeys> MapRedisKeys(string postId, IEnumerable<string> followers)
        {
            var retVal = new List<FollowerRedisKeys>();

            foreach (var follower in followers)
            {
                var item = new FollowerRedisKeys
                {
                    //newsfeed:user      
                    PostFeedKey = GetEntryKey(FeedPrefix, follower),
                    //userfeed:postId
                    PostActionsKey = GetEntryKey(FeedPrefix, string.Concat(follower, ":", postId)),
                    PostId = postId
                };

                retVal.Add(item);
            }
            return retVal;
        }

        private string MapToActivity(NewsfeedEventModel newsFeedEntry)
        {
            return string.Concat(newsFeedEntry.By, ":", (short)newsFeedEntry.ActionType);
        }
    }
}
