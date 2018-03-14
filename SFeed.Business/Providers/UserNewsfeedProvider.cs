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
                var cacheModel = new NewsfeedCacheModel
                {
                    By = newsFeedEntry.By,
                    FeedType = newsFeedEntry.FeedType,
                    ReferencePostId = newsFeedEntry.ReferencePostId
                };
                feedCacheRepo.AddPost(cacheModel, followers);
            }
        }

        public IEnumerable<NewsfeedWallPostModel> GetUserFeed(string userId, int skip, int take)
        {
            return feedCacheRepo.GetUserFeed(userId, skip, take);
        }

        public void RemoveNewsfeedItem(NewsfeedItem newsFeedEntry)
        {
            if (newsFeedEntry.FeedType == NewsfeedType.wallpost)
            {
                RemovePost(newsFeedEntry);
            }
            else
            {
                var followers = GetFollowers(newsFeedEntry.By, newsFeedEntry.WallOwner);

                if (followers.Any())
                {
                    var cacheModel = new NewsfeedCacheModel
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
                feedCacheRepo.RemovePost(newsFeedEntry.ReferencePostId, followers);
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






        //private int latestCommentCount = 3;

        //public void UpdateComment(string postId, CommentCacheModel comment)
        //{
        //    var existingRecord = wallPostCacheRepo.GetItem(postId);
        //    var latestComments = existingRecord.LatestComments;
        //    if (latestComments != null)
        //    {
        //        var commentIndex = latestComments.FindIndex(c => c.Id == comment.Id);
        //        if (commentIndex >= 0)
        //        {
        //            latestComments[commentIndex] = comment;
        //        }
        //    }
        //    latestComments.Add(comment);
        //    existingRecord.LatestComments = latestComments;
        //    wallPostCacheRepo.UpdateItem(postId, existingRecord);
        //}

        //public void DeleteComment(string postId, long commentId, string commentBy)
        //{
        //    var existingRecord = wallPostCacheRepo.GetItem(postId);
        //    var latestComments = existingRecord.LatestComments;
        //    if (latestComments != null)
        //    {
        //        var commentIndex = latestComments.FindIndex(c => c.Id == commentId);
        //        if (commentIndex >= 0)
        //        {
        //            //if (latestComments.Count < latestCommentCount)
        //            //{
        //            latestComments.RemoveAt(commentIndex);

        //            //}
        //            //else
        //            //{
        //            //    //missing comments can be reset here
        //            //}
        //        }
        //    }
        //    DeleteNewsfeedItem(new NewsfeedEntry
        //    {
        //        EntryTypeId = (short)NewsfeedEntryType.comment,
        //        ReferencePostId = postId,
        //        By = commentBy
        //    });
        //}

        //public void LikePost(string postId, string likedBy)
        //{
        //    var existingRecord = wallPostCacheRepo.GetItem(postId);
        //    existingRecord.Likes = existingRecord.Likes ?? new List<string>();
        //    existingRecord.Likes.Add(likedBy);
        //    wallPostCacheRepo.UpdateItem(postId, existingRecord);
        //    AddNewsfeedItem(new NewsfeedEntry
        //    {
        //        EntryTypeId = (short)NewsfeedEntryType.like,
        //        By = likedBy,
        //        EventDate = DateTime.Now,
        //        ReferencePostId = postId
        //    });
        //}

        //public void UnlikePost(string postId, string unlikedBy)
        //{
        //    var existingRecord = wallPostCacheRepo.GetItem(postId);
        //    existingRecord.Likes.Remove(unlikedBy);
        //    wallPostCacheRepo.UpdateItem(postId, existingRecord);
        //    DeleteNewsfeedItem(new NewsfeedEntry
        //    {
        //        EntryTypeId = (short)NewsfeedEntryType.like,
        //        By = unlikedBy,
        //        ReferencePostId = postId
        //    });
        //}

        //public void AddPost(WallPostCacheModel wallPost)
        //{
        //    wallPostCacheRepo.AddItem(wallPost);
        //    AddNewsfeedItem(new NewsfeedEntry
        //    {
        //        EntryTypeId = (short)NewsfeedEntryType.wallpost,
        //        By = wallPost.PostedBy,
        //        EventDate = DateTime.Now,
        //        ReferencePostId = wallPost.Id
        //    });
        //}

        //public void UpdatePost(string postId, string body, WallPostType postType)
        //{
        //    var existingRecord = wallPostCacheRepo.GetItem(postId);
        //    existingRecord.Body = body;
        //    existingRecord.PostType = (short)postType;
        //    wallPostCacheRepo.UpdateItem(postId, existingRecord);
        //}
        //public void DeletePost(string postId)
        //{
        //    wallPostCacheRepo.RemoveItem(postId);
        //    DeleteNewsfeedItem(new NewsfeedEntry
        //    {
        //        EntryTypeId = (short)NewsfeedEntryType.wallpost,
        //        ReferencePostId = postId
        //    });
        //}

        //public void AddComment(string postId, CommentCacheModel comment)
        //{
        //    var existingRecord = wallPostCacheRepo.GetItem(postId);
        //    var latestComments = existingRecord.LatestComments;
        //    if (latestComments == null)
        //    {
        //        latestComments = new List<CommentCacheModel>();
        //    }
        //    else if (latestComments.Count > latestCommentCount)
        //    {
        //        latestComments.RemoveAt(0);
        //    }
        //    latestComments.Add(comment);
        //    existingRecord.LatestComments = latestComments;
        //    existingRecord.CommentCount++;
        //    wallPostCacheRepo.UpdateItem(postId, existingRecord);

        //    AddNewsfeedItem(new NewsfeedEntry
        //    {
        //        EntryTypeId = (short)NewsfeedEntryType.comment,
        //        By = comment.Body,
        //        EventDate = DateTime.Now,
        //        ReferencePostId = postId
        //    });
        //}


    }
}
