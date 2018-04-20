﻿using SFeed.Core.Infrastructure.Caching;
using System;
using System.Collections.Generic;
using SFeed.Core.Models.Caching;
using SFeed.RedisRepository.Base;
using SFeed.Core.Models.Wall;
using SFeed.Core.Models.Newsfeed;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisNewsfeedResponseRepository : RedisRepositoryBase, INewsfeedResponseCacheRepository
    {
        public string FeedPrefix => RedisNameConstants.FeedRepoPrefix;

        ICommentCountCacheRepository commentCountRepo;
        ILikeCountCacheRepository entryLikeRepo;
        IWallPostCacheRepository wallPostRepo;

        public RedisNewsfeedResponseRepository() : this(
            new RedisCommentCountRepository()
            , new RedisEntryLikeRepository(),
            new RedisWallPostRepository())
        {

        }
        public RedisNewsfeedResponseRepository(ICommentCountCacheRepository commentCountRepo,
             ILikeCountCacheRepository entryLikeRepo,
             IWallPostCacheRepository wallPostRepo)
        {
            this.commentCountRepo = commentCountRepo;
            this.entryLikeRepo = entryLikeRepo;
            this.wallPostRepo = wallPostRepo;
        }

        public IEnumerable<NewsfeedResponseModel> GetUserFeed(string userId, int skip, int take)
        {
            var userFeedKey = GetEntryKey(FeedPrefix, userId);
            List<NewsfeedResponseModel> responseItems = new List<NewsfeedResponseModel>();
            var startIndex = skip;
            var counter = 0;

            using (var client = GetClientInstance())
            {
                var postIds = client.GetRangeFromSortedSetByHighestScore(userFeedKey, skip, take);
                if (postIds != null && postIds.Count > 0)
                {
                    foreach (var postId in postIds)
                    {
                        //Read post from cache
                        var currentPost = wallPostRepo.GetPost(postId);
                        if (currentPost != null)
                        {
                            var userActionsOnPostKey = GetEntryKey(FeedPrefix, string.Concat(userId, ":", postId));
                            //10000 is just a random rank assumed to be the max events on a post
                            var userActionsOnPost = client.GetRangeFromSortedSet(userActionsOnPostKey, 0, 10000);
                            if (userActionsOnPost != null && userActionsOnPost.Count > 0)
                            {
                                var mappedActions = new List<NewsfeedAction>();
                                foreach (var action in userActionsOnPost)
                                {
                                    var values = action.Split(':');
                                    mappedActions.Add(new NewsfeedAction { Action = (NewsfeedActionType)Convert.ToInt16(values[1]), By = values[0] });
                                }
                                var totalCommentCount = commentCountRepo.GetCommentCount(currentPost.Id);
                                var totalLikeCount = entryLikeRepo.GetPostLikeCount(currentPost.Id);

                                var model = new NewsfeedResponseModel
                                {
                                    Body = currentPost.Body,
                                    CreatedDate = currentPost.CreatedDate,
                                    Id = currentPost.Id,
                                    ModifiedDate = currentPost.ModifiedDate,
                                    PostedBy = currentPost.PostedBy,
                                    PostType = currentPost.PostType,
                                    WallOwner = new WallModel { OwnerId = currentPost.TargetWall.Id, WallOwnerType = (WallType)currentPost.TargetWall.WallOwnerTypeId},
                                    FeedDescription = mappedActions,
                                    LikeCount = totalLikeCount,
                                    CommentCount = totalCommentCount
                                };
                                responseItems.Add(model);

                                counter++;
                                if (counter == take) break;

                            }
                            else
                            {
                                client.RemoveItemFromSortedSet(userFeedKey, postId);
                                client.RemoveEntry(userActionsOnPostKey);
                            }
                        }
                        else
                        {
                            client.RemoveItemFromSortedSet(userFeedKey, postId);
                        }
                    }
                   
                }
                return responseItems;
            }
        }
    }
}
