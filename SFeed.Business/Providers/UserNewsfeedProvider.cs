﻿using SFeed.Core.Infrastructure.Providers;
using System.Collections.Generic;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;
using SFeed.Core.Models.Newsfeed;
using AutoMapper;
using SFeed.Core.Models;
using SFeed.Core.Models.Caching;
using SFeed.Core.Models.WallPost;
using System.Linq;
using System;

namespace SFeed.Business.Providers
{
    public sealed class UserNewsfeedProvider : INewsfeedProvider
    {
        ICacheListRepository<NewsfeedEntry> feedCacheRepo;
        ITypedCacheRepository<WallPostCacheModel> wallPostCacheRepo;
        IFollowerProvider userFollowerProvider;
        private int latestCommentCount = 3;

        public UserNewsfeedProvider() : this(
            new RedisUserFeedRepository(), 
            new RedisWallPostRepository(),
            new FollowerProvider())
        {

        }
        public UserNewsfeedProvider(ICacheListRepository<NewsfeedEntry> feedCacheRepo,
            ITypedCacheRepository<WallPostCacheModel> wallPostCacheRepo,
            IFollowerProvider userFollowerProvider)
        {
            this.feedCacheRepo = feedCacheRepo;
            this.wallPostCacheRepo = wallPostCacheRepo;
            this.userFollowerProvider = userFollowerProvider;
        }
         


        private void AddNewsfeedItem(NewsfeedEntry entry)
        {
            var followers = GetFollowers(entry);

            foreach (var userId in followers)
            {
                var userFeeds = feedCacheRepo.GetList(userId);
                if (!userFeeds.Any(f => f.ReferencePostId == entry.ReferencePostId))
                {
                    feedCacheRepo.PrependToList(userId, entry);
                }
            }
        }

        private void DeleteNewsfeedItem(NewsfeedEntry entry)
        {
            var followers = GetFollowers(entry);

            foreach (var userId in followers)
            {
                feedCacheRepo.RemoveFromList(userId, entry);
            }
        }

        private IEnumerable<NewsfeedResponseItem> GetUserNewsfeed(string userId)
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


        public void AddPost(WallPostCacheModel wallPost)
        {
            wallPostCacheRepo.AddItem(wallPost);
            AddNewsfeedItem(new NewsfeedEntry
            {
                EntryTypeId = (short)NewsfeedEntryType.wallpost,
                By = wallPost.PostedBy,
                EventDate = DateTime.Now,
                ReferencePostId = wallPost.Id
            });
        }

        public void UpdatePost(string postId, string body, WallPostType postType)
        {
            var existingRecord = wallPostCacheRepo.GetItem(postId);
            existingRecord.Body = body;
            existingRecord.PostType = (short)postType;
            wallPostCacheRepo.UpdateItem(postId, existingRecord);
        }
        public void DeletePost(string postId)
        {
            wallPostCacheRepo.RemoveItem(postId);
            DeleteNewsfeedItem(new NewsfeedEntry
            {
                EntryTypeId = (short)NewsfeedEntryType.wallpost,
                ReferencePostId = postId
            });
        }

        public void AddComment(string postId, CommentCacheModel comment)
        {
            var existingRecord = wallPostCacheRepo.GetItem(postId);
            var latestComments = existingRecord.LatestComments;
            if (latestComments == null)
            {
                latestComments = new List<CommentCacheModel>();
            }
            else if (latestComments.Count > latestCommentCount)
            {
                latestComments.RemoveAt(0);
            }
            latestComments.Add(comment);
            existingRecord.LatestComments = latestComments;
            existingRecord.CommentCount++;
            wallPostCacheRepo.UpdateItem(postId, existingRecord);

            AddNewsfeedItem(new NewsfeedEntry
            {
                EntryTypeId = (short)NewsfeedEntryType.comment,
                By = comment.Body,
                EventDate = DateTime.Now,
                ReferencePostId = postId
            });
        }

        public void UpdateComment(string postId, CommentCacheModel comment)
        {
            var existingRecord = wallPostCacheRepo.GetItem(postId);
            var latestComments = existingRecord.LatestComments;
            if (latestComments != null)
            {
                var commentIndex = latestComments.FindIndex(c => c.Id == comment.Id);
                if (commentIndex >= 0)
                {
                    latestComments[commentIndex] = comment;
                }
            }
            latestComments.Add(comment);
            existingRecord.LatestComments = latestComments;
            wallPostCacheRepo.UpdateItem(postId, existingRecord);
        }

        public void DeleteComment(string postId, long commentId, string commentBy)
        {
            var existingRecord = wallPostCacheRepo.GetItem(postId);
            var latestComments = existingRecord.LatestComments;
            if (latestComments != null)
            {
                var commentIndex = latestComments.FindIndex(c => c.Id == commentId);
                if (commentIndex >= 0)
                {
                    //if (latestComments.Count < latestCommentCount)
                    //{
                        latestComments.RemoveAt(commentIndex);

                    //}
                    //else
                    //{
                    //    //missing comments can be reset here
                    //}
                }
            }
            DeleteNewsfeedItem(new NewsfeedEntry
            {
                EntryTypeId = (short)NewsfeedEntryType.comment,
                ReferencePostId = postId,
                By = commentBy
            });
        }

        public void LikePost(string postId, string likedBy)
        {
            var existingRecord = wallPostCacheRepo.GetItem(postId);
            existingRecord.Likes = existingRecord.Likes ?? new List<string>();
            existingRecord.Likes.Add(likedBy);
            wallPostCacheRepo.UpdateItem(postId, existingRecord);
            AddNewsfeedItem(new NewsfeedEntry
            {
                EntryTypeId = (short)NewsfeedEntryType.like,
                By = likedBy,
                EventDate = DateTime.Now,
                ReferencePostId = postId
            });
        }

        public void UnlikePost(string postId, string unlikedBy)
        {
            var existingRecord = wallPostCacheRepo.GetItem(postId);
            existingRecord.Likes.Remove(unlikedBy);
            wallPostCacheRepo.UpdateItem(postId, existingRecord);
            DeleteNewsfeedItem(new NewsfeedEntry
            {
                EntryTypeId = (short)NewsfeedEntryType.like,
                By = unlikedBy,
                ReferencePostId = postId
            });
        }

       
    }
}
