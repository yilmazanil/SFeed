using SFeed.Core.Infrastructure.Repository.Caching;
using SFeed.Core.Models.Newsfeed;
using SFeed.RedisRepository.Base;
using System.Collections.Generic;
using System;

namespace SFeed.RedisRepository.Implementation
{
   

    public class RedisNewsfeedEntryRepository : RedisRepositoryBase, INewsfeedCacheRepository
    {
        public string FeedPrefix => RedisNameConstants.FeedRepoPrefix;
        public string ActivityIdPrefix => RedisNameConstants.ActivityIdPrefix;
        public string ActivityPrefix => RedisNameConstants.ActivityPrefix;

        public void AddEntry(NewsfeedEntry entry, IEnumerable<string> followers)
        {
            var activityEntry = ConvertToActivity(entry);
            var keys = MapKeys(entry, followers);

            using (var client = GetClientInstance())
            {
                using (var trans = client.CreateTransaction())
                {
                    foreach (var key in keys)
                    {
                        trans.QueueCommand(r => r.AddItemToSet(key.PostFeedKey, entry.ReferencePostId));
                        //uses zincr adds if not exists
                        trans.QueueCommand(r => r.IncrementItemInSortedSet(key.PostActionsKey, activityEntry, 1));
                    }
                    trans.Commit();
                }
            }
        }

        public void RemoveEntry(NewsfeedEntry entry, IEnumerable<string> followers)
        {
            var activityEntry = ConvertToActivity(entry);

            var keys = MapKeys(entry, followers);

            using (var client = GetClientInstance())
            {
                using (var trans = client.CreateTransaction())
                {
                    //List<Action<IRedisClient>> removalCommands = new List<Action<IRedisClient>>();
                    foreach (var key in keys)
                    {
                        //Decrement user activity rank in associated post details of a follower feed
                        //If user commented twice and deleted one of them
                        //Post should still be displayed
                        trans.QueueCommand(r => r.IncrementItemInSortedSet(key.PostActionsKey, activityEntry, -1));
                        //remove the details of a post if it has a rank below 0, 10 could be any negative value
                        trans.QueueCommand(r => r.RemoveRangeFromSortedSet(key.PostActionsKey, -10, 0));
                        //trans.QueueCommand(r => r.GetSetCount(key.PostActionsKey), r=>
                        //{
                        //    if (r < 1)
                        //    {
                        //        removalCommands.Add(c => c.RemoveItemFromSet(key.PostFeedKey, key.PostId));
                        //    }

                        //});

                        trans.Commit();
                    }

                    //cleanup
                    //foreach (var command in removalCommands)
                    //{
                    //    trans.QueueCommand(command);
                    //}
                    //trans.Commit();
                }
            }
        }

        /// <summary>
        /// When user unfollows a group, postIds by group must be provided
        /// </summary>
        /// <param name="follower"></param>
        /// <param name="postIds"></param>
        public void RemovePostsFromUser(string follower, IEnumerable<string> postIds)
        {
            //slowest method alternative could be fetching the 
            //activities from alternative data storage mechanism

            var userFeedKey = GetEntryKey(FeedPrefix, follower);
            var postDetailKeys = new Dictionary<string, string>();
            foreach (var postId in postIds)
            {
                postDetailKeys.Add(postId, GetEntryKey(follower, postId));
            }

            using (var client = GetClientInstance())
            {
                using (var trans = client.CreateTransaction())
                {
                    foreach (var postDetailKey in postDetailKeys)
                    {
                        trans.QueueCommand(r => r.RemoveItemFromSet(userFeedKey, postDetailKey.Key));
                        trans.QueueCommand(r => r.Remove(postDetailKey.Value));
                    }
                    trans.Commit();
                }


            }
        }

        private ActivityEntry ConvertToActivityEntry(string activityStringRepresentation)
        {
            var activity = activityStringRepresentation.Split(':');
            return new ActivityEntry
            {
                By = activity[0],
                ActivityId = Convert.ToInt16(activity[1])
            };
        }

        private string ConvertToActivity(NewsfeedEntry newsFeedEntry)
        {
            return string.Concat(newsFeedEntry.By, ":", (short)newsFeedEntry.FeedType);
        }

        private IEnumerable<FollowerRedisKeys> MapKeys(NewsfeedEntry entry, IEnumerable<string> followers)
        {
            var retVal = new List<FollowerRedisKeys>();

            foreach (var follower in followers)
            {
                var item = new FollowerRedisKeys
                {
                    //newsfeed:user      
                    PostFeedKey = GetEntryKey(FeedPrefix, follower),
                    //userfeed:postId
                    PostActionsKey = GetEntryKey(follower, entry.ReferencePostId),
                    PostId = entry.ReferencePostId
                };

                retVal.Add(item);
            }
            return retVal;

        }
    }
}
