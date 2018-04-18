using SFeed.Core.Infrastructure.Caching;
using System;
using System.Collections.Generic;
using SFeed.Core.Models.Caching;
using SFeed.RedisRepository.Base;

namespace SFeed.RedisRepository.Implementation
{
    public class RedisNewsfeedResponseRepository : RedisRepositoryBase, INewsfeedResponseCacheRepository
    {
        public string FeedPrefix => RedisNameConstants.FeedRepoPrefix;

        ICommentCacheRepository commentRepo;
        IEntryLikeCacheRepository entryLikeRepo;
        IWallPostCacheRepository wallPostRepo;

        public RedisNewsfeedResponseRepository() : this(
            new RedisCommentRepository()
            ,new RedisEntryLikeRepository(),
            new RedisWallPostRepository())
        {

        }
        public RedisNewsfeedResponseRepository(ICommentCacheRepository commentRepo,
             IEntryLikeCacheRepository entryLikeRepo,
             IWallPostCacheRepository wallPostRepo)
        {
            this.commentRepo = commentRepo;
            this.entryLikeRepo = entryLikeRepo;
            this.wallPostRepo = wallPostRepo;
        }

        public IEnumerable<NewsfeedWallPostModel> GetUserFeed(string userId, int skip, int take)
        {
            var userFeedKey = GetEntryKey(FeedPrefix, userId);
            List<string> postIds;
            List<NewsfeedWallPostModel> responseItems = new List<NewsfeedWallPostModel>();
            var startIndex = skip;

            using (var client = GetClientInstance())
            {
                var postIds = client.GetRangeFromList(userFeedKey, skip, take);
                if (postIds != null && postIds.Count > 0)
                {
                    foreach (var postId in postIds)
                    {
                        var currentPost = wallPostRepo.GetPost(postId);
                        if (currentPost != null)
                        {
                            var userActionsOnPostKey = GetEntryKey(FeedPrefix, string.Concat(userId, ":", postId));
                            //10000 is just a random rank assumed to be the max events on a post
                            var userActionsOnPost = client.GetRangeFromSortedSet(userActionsOnPostKey, 0, 10000);
                            if (userActionsOnPost != null && userActionsOnPost.Count > 0)
                            {
                                var totalCommentCount = commentRepo.GetCommentCount(currentPost.Id);
                                var totalLikeCount = entryLikeRepo.GetPostLikeCount(currentPost.Id);
                                var latestComments = commentRepo.GetLatestComments(currentPost.Id);

                                var model = new NewsfeedWallPostModel
                                {
                                    Body = currentPost.Body,
                                    CreatedDate = currentPost.CreatedDate,
                                    Id = currentPost.Id,
                                    ModifiedDate = currentPost.ModifiedDate,
                                    PostedBy = currentPost.PostedBy,
                                    PostType = currentPost.PostType,
                                    WallOwner = currentPost.TargetWall,
                                    FeedDescription = actions.ToList(),
                                    LikeCount = likeCount,
                                    CommentCount = commentCount,
                                    LatestComments = latestComments
                                };
                                responseItems.Add(model);
                            }
                            else
                            {
                                client.RemoveEntry(postFeedReasonKey);
                            }
                        }
                        else
                        {
                            client.RemoveItemFromList(userFeedKey, postId);
                        }
                    }
                    return responseItems;
                }
            }
        }
    }
}
