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

namespace SFeed.Business.Providers
{
    //TODO: Include wallpost owner followers for wallpost
    public class UserNewsfeedProvider : INewsfeedProvider
    {
        ICacheListRepository<NewsfeedEntry> feedCacheRepo;
        IFollowerProvider followerProvider;
        INewsfeedResponseProvider newsFeedResponseProvider;

        public UserNewsfeedProvider() : this(
            new RedisUserFeedRepository(),
            new FollowerProvider(),
            new UserNewsfeedResponseProvider())
        {

        }
        public UserNewsfeedProvider(
            ICacheListRepository<NewsfeedEntry> feedCacheRepo,
            IFollowerProvider followerProvider,
            INewsfeedResponseProvider newsFeedResponseProvider)
        {
            this.feedCacheRepo = feedCacheRepo;
            this.followerProvider = followerProvider;
            this.newsFeedResponseProvider = newsFeedResponseProvider;
        }       
       
        public IEnumerable<NewsfeedResponseItem> GetUserNewsfeed(string userId)
        {
            return newsFeedResponseProvider.GetUserNewsfeed(userId);
        }

        public void AddNewsfeedItem(NewsfeedEntry newsFeedEntry)
        {
            var followers = GetFollowers(new List<Actor> { new Actor { ActorTypeId = (short)ActorType.user, Id = newsFeedEntry.By } });

            foreach (var userId in followers)
            {
                feedCacheRepo.PrependItem(userId, newsFeedEntry);
            }
        }

        public void AddNewsfeedItem(NewsfeedEntry newsFeedEntry, List<Actor> actors)
        {
            var followers = GetFollowers(actors);

            foreach (var userId in followers)
            {
                feedCacheRepo.PrependItem(userId, newsFeedEntry);
            }
        }

        public void RemoveNewsfeedItem(string actionBy, Predicate<NewsfeedEntry> where)
        {
            var followers = GetFollowers(new List<Actor> { new Actor { ActorTypeId = (short)ActorType.user, Id = actionBy } });

            foreach (var userId in followers)
            {
                feedCacheRepo.RemoveItem(userId, where);
            }
        }
        public void RemoveNewsfeedItem(List<Actor> actors, Predicate<NewsfeedEntry> where)
        {
            var followers = GetFollowers(actors);

            foreach (var userId in followers)
            {
                feedCacheRepo.RemoveItem(userId, where);
            }
        }

        private IEnumerable<string> GetFollowers(IEnumerable<Actor> actors)
        {
            return followerProvider.GetFollowers(actors);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (feedCacheRepo != null)
                {
                    feedCacheRepo.Dispose();
                }
                if (followerProvider != null)
                {
                    followerProvider.Dispose();
                }

            }
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
