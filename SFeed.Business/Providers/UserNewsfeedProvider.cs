using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.RedisRepository;
using SFeed.Core.Models.Newsfeed;
using AutoMapper;
using SFeed.Core.Models;
using System;
using SFeed.Core.Infrastructure.Repository;
using SFeed.Core.Models.Caching;
using System.Linq.Expressions;
using SFeed.Core.Infrastructure.Repository.Caching;
using SFeed.RedisRepository.Implementation;
using System.Linq;

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
       
        public IEnumerable<NewsfeedResponseItem> GetUserNewsfeed(string userId)
        {
            throw new NotImplementedException();
            return newsFeedResponseProvider.GetUserNewsfeed(userId);
        }

        public void AddNewsfeedItem(NewsfeedEntry newsFeedEntry)
        {

            var followers = GetFollowers(new List<WallOwner> { new WallOwner { ActorTypeId = (short)WallOwnerType.user, Id = newsFeedEntry.By } });

            foreach (var userId in followers)
            {
                feedCacheRepo.PrependItem(userId, newsFeedEntry);
            }
        }

        public void AddNewsfeedItem(NewsfeedEntry newsFeedEntry, List<WallOwner> actors)
        {
            var followers = GetFollowers(actors);

            foreach (var userId in followers)
            {
                feedCacheRepo.PrependItem(userId, newsFeedEntry);
            }
        }

        public void RemoveNewsfeedItem(string actionBy, Predicate<NewsfeedEntry> where)
        {
            var followers = GetFollowers(new List<WallOwner> { new WallOwner { ActorTypeId = (short)WallOwnerType.user, Id = actionBy } });

            foreach (var userId in followers)
            {
                feedCacheRepo.RemoveItem(userId, where);
            }
        }
        public void RemoveNewsfeedItem(List<WallOwner> actors, Predicate<NewsfeedEntry> where)
        {
            var followers = GetFollowers(actors);

            foreach (var userId in followers)
            {
                feedCacheRepo.RemoveItem(userId, where);
            }
        }

        private IEnumerable<string> GetFollowers(IEnumerable<WallOwner> actors)
        {
            return followerProvider.GetFollowers(actors);
        }

        public void AddNewsfeedItem(NewsfeedEntry newsFeedEntry, WallOwner wallowner)
        {
            IEnumerable<string> followers = followerProvider.GetUserFollowers(newsFeedEntry.By);
            if (wallowner.WallOwnerType == WallOwnerType.user && !string.Equals(wallowner.Id, newsFeedEntry.By))
            {
                var wallOwnerFollowers = followerProvider.GetUserFollowers(wallowner.Id);
                followers = followers.Union(wallOwnerFollowers).Distinct();

            }
            else
            {
                if (wallowner.WallOwnerType != WallOwnerType.privateGroup)
                {
                    var wallOwnerFollowers = followerProvider.GetUserFollowers(wallowner.Id);
                    followers = followers.Union(wallOwnerFollowers).Distinct();
                }
                else
                {
                    followers = followerProvider.GetGroupFollowers(wallowner.Id);
                }
            }

            feedCacheRepo.AddEntry(newsFeedEntry, followers);
        }

        public void RemoveNewsfeedItem(NewsfeedEntry newsFeedEntry)
        {

            feedCacheRepo.RemoveEntry(newsFeedEntry);
        }

        public void RemoveFeedsFromUser(string userId, WallOwner fromWall)
        {
            throw new NotImplementedException();
        }

        public void RemoveFeedsFromUser(string userId, string fromUser)
        {
            throw new NotImplementedException();
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
