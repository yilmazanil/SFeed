using SFeed.Core.Infrastructure.Repository.Caching;
using System;
using SFeed.Core.Models.Newsfeed;
using SFeed.RedisRepository.Base;
using System.Collections.Generic;
using ServiceStack.Redis;
using ServiceStack;

namespace SFeed.RedisRepository.Implementation
{
    public class ActivityEntry
    {
        short ActivityType { get; set; }
        string ActivityBy { get; set; }
    }



    public class RedisNewsfeedEntryRepository : RedisRepositoryBase, INewsfeedCacheRepository
    {
        public string FeedPrefix => RedisNameConstants.FeedRepoPrefix;
        public string ActivityIdPrefix => RedisNameConstants.ActivityIdPrefix;
        public string ActivityPrefix => RedisNameConstants.ActivityPrefix;

        public void AddEntry(NewsfeedEntry entry, IEnumerable<string> followers)
        {
            var followerKeys = new Dictionary<string, string>();
            //user:comment
            var activity = entry.ToJson();

            foreach (var follower in followers)
            {
                //newsfeed:user
                var followerNewsfeedKey = GetEntryKey(FeedPrefix, follower);
                //userfeed:postId
                var activityPrefix = GetActivityPrefix(follower, entry.ReferencePostId);

                followerKeys.Add(followerNewsfeedKey, activityPrefix);
            }

            using (var client = GetClientInstance())
            {
                using (var trans = client.CreateTransaction())
                {
                    foreach (var keys in followerKeys)
                    {

                        trans.QueueCommand(r => r.AddItemToSet(keys.Key, entry.ReferencePostId));
                        //uses zincr adds if not exists
                        trans.QueueCommand(r => r.IncrementItemInSortedSet(keys.Value, activity, 1));
                    }
                    trans.Commit();
                }
            }
        }

        public void RemoveEntry(NewsfeedEntry entry, IEnumerable<string> followers)
        {
            var followerKeys = new Dictionary<string, string>();

            var activity = entry.ToJson();


            foreach (var follower in followers)
            {
                //newsfeed:user
                var followerNewsfeedKey = GetEntryKey(FeedPrefix, follower);
                //userfeed:postId
                var activityPrefix = GetActivityPrefix(follower, entry.ReferencePostId);

                followerKeys.Add(followerNewsfeedKey, activityPrefix);
            }


            using (var client = GetClientInstance())
            {
                using (var trans = client.CreateTransaction())
                {
                    foreach (var followerKey in followerKeys)
                    {
                        trans.QueueCommand(r => r.IncrementItemInSortedSet(followerKey.Value, activity, -1));
                        trans.QueueCommand(r =>
                        {
                            r.RemoveRangeFromSortedSet(followerKey.Value, -10, 0);
                            if (r.GetSetCount(followerKey.Value) < 1)
                            {
                                r.RemoveItemFromSet(followerKey.Key, entry.ReferencePostId);
                            }
                        });
                    }
                }
            }
        }

        public void RemoveEntriesByUser(string user, Dictionary<string, NewsfeedEntry> userActivitesToRemove)
        {
            
            //items: action : count
            var userNewsfeedKey = GetEntryKey(FeedPrefix, user);

            using (var client = GetClientInstance())
            {
                using (var trans = client.CreateTransaction())
                {
                    var newsFeedPosts = client.GetAllItemsFromSortedSet(userNewsfeedKey);
                }


            }
        }



        private void GetActivities(IRedisClient client, IEnumerable<string> following)
        {
            //usernewsfeed
            //
        }

        private string GetActivityPrefix(string follower, string postId)
        {
            return string.Concat(follower, ":", postId);
        }
    }
}
