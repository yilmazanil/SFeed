using SFeed.RedisRepository.Base;
using System.Collections.Generic;
using System;
using SFeed.Core.Infrastructure.Caching;
using SFeed.Core.Models.Newsfeed;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisNewsfeedEntryRepository : RedisRepositoryBase, INewsfeedCacheRepository
    {
        public string FeedPrefix => RedisNameConstants.FeedRepoPrefix;
        public string ActivityIdPrefix => RedisNameConstants.ActivityIdPrefix;
        public string ActivityPrefix => RedisNameConstants.ActivityPrefix;

        ICommentCacheRepository commentRepo;
        IEntryLikeCacheRepository entryLikeRepo;
        IWallPostCacheRepository wallPostRepo;

        public RedisNewsfeedEntryRepository() : this(new RedisCommentRepository()
            ,new RedisEntryLikeRepository(), new RedisWallPostRepository())
        {

        }
        public RedisNewsfeedEntryRepository(ICommentCacheRepository commentRepo, 
             IEntryLikeCacheRepository entryLikeRepo,
             IWallPostCacheRepository wallPostRepo)
        {
            this.commentRepo = commentRepo;
            this.entryLikeRepo = entryLikeRepo;
            this.wallPostRepo = wallPostRepo;
        }
        public void AddEvent(NewsfeedCacheModel entry, IEnumerable<string> followers)
        {
            var activityEntry = MapToActivity(entry);
            var keys = MapKeys(entry.ReferencePostId, followers);

            using (var client = GetClientInstance())
            {
                using (var trans = client.CreateTransaction())
                {
                    foreach (var key in keys)
                    {
                        //Add post id to user feed, overwrites if exists
                        trans.QueueCommand(r => r.PrependItemToList(key.PostFeedKey, entry.ReferencePostId));
                        //keep only last 100 newsfeed items
                        trans.QueueCommand(r => r.TrimList(key.PostFeedKey, 0, 100));
                        //Increment rank of feed item by 1 uses zincr adds if not exists
                        trans.QueueCommand(r => r.IncrementItemInSortedSet(key.PostActionsKey, activityEntry, 1));
                    }
                    trans.Commit();
                }
            }
        }

        public void RemoveEvent(NewsfeedCacheModel entry, IEnumerable<string> followers)
        {
            var activityEntry = MapToActivity(entry);
            var keys = MapKeys(entry.ReferencePostId, followers);
            var isPostRemoval = entry.FeedType == NewsfeedType.wallpost;

            using (var client = GetClientInstance())
            {
                using (var trans = client.CreateTransaction())
                {
                    if (isPostRemoval)
                    {
                        foreach (var key in keys)
                        {
                            //Add post id to user feed, overwrites if exists
                            trans.QueueCommand(r => r.RemoveItemFromList(key.PostFeedKey, entry.ReferencePostId));
                            //Increment rank of feed item by 1 uses zincr adds if not exists
                            trans.QueueCommand(r => r.Remove(key.PostActionsKey));
                        }
                    }
                    else
                    {
                        foreach (var key in keys)
                        {
                            //Decrement user activity rank in associated post details of a follower feed
                            //If user commented twice and deleted one of them
                            //Post should still be displayed
                            trans.QueueCommand(r => r.IncrementItemInSortedSet(key.PostActionsKey, activityEntry, -1));
                            //remove the details of a post if it has a rank below 0, 10 could be any negative value
                            trans.QueueCommand(r => r.RemoveRangeFromSortedSet(key.PostActionsKey, -10, 0));
                            //10000 can be any value for a random max score
                            trans.Commit();
                        }
                    }
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

        private string MapToActivity(NewsfeedCacheModel newsFeedEntry)
        {
            return string.Concat(newsFeedEntry.By, ":", (short)newsFeedEntry.FeedType);
        }

        private IEnumerable<FollowerRedisKeys> MapKeys(string postId, IEnumerable<string> followers)
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

        //public IEnumerable<NewsfeedWallPostModel> GetUserFeed(string userId, int skip, int take)
        //{
        //    var userFeedKey = GetEntryKey(FeedPrefix, userId);
        //    List<string> postIds;
        //    List<NewsfeedWallPostModel> responseItems = new List<NewsfeedWallPostModel>();

        //    using (var client = GetClientInstance())
        //    {
        //        postIds = client.GetRangeFromList(userFeedKey, skip, take);
        //        foreach (var postId in postIds)
        //        {
        //            var post = wallPostRepo.GetPost(postId);
        //            if (post != null)
        //            {
        //                var postFeedReasonKey = GetEntryKey(FeedPrefix, string.Concat(userId, ":", postId));
        //                var actions = client.GetAllItemsFromSortedSet(postFeedReasonKey);
        //                if (actions != null)
        //                {
        //                    var latestComments = commentRepo.GetLatestComments(post.Id);
        //                    var commentCount = commentRepo.GetCommentCount(post.Id);
        //                    var likeCount = entryLikeRepo.GetPostLikeCount(post.Id);
        //                    var model = new NewsfeedWallPostModel
        //                    {
        //                        Body = post.Body,
        //                        CreatedDate = post.CreatedDate,
        //                        Id = post.Id,
        //                        ModifiedDate = post.ModifiedDate,
        //                        PostedBy = post.PostedBy,
        //                        PostType = post.PostType,
        //                        TargetWall = post.TargetWall,
        //                        FeedDescription = actions.ToList(),
        //                        LikeCount = likeCount,
        //                        CommentCount = commentCount,
        //                        LatestComments = latestComments
        //                    };
        //                    responseItems.Add(model);
        //                }
        //                else
        //                {
        //                    client.RemoveEntry(postFeedReasonKey);
        //                }
        //            }
        //            else
        //            {
        //                client.RemoveItemFromList(userFeedKey, postId);
        //            }
        //        }
        //        return responseItems;
        //    }
        //}
    }
}
