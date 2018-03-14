using SFeed.Core.Infrastructure.Caching;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Infrastructure.Repository;
using SFeed.RedisRepository.Implementation;
using SFeed.SqlRepository.Implementation;
using System.Collections.Generic;
using SFeed.Core.Models.EntryLike;
using log4net;
using System;

namespace SFeed.Business.Providers
{
    public class EntryLikeProvider : IEntryLikeProvider
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(EntryLikeProvider));

        IEntryLikeRepository entryLikeRepo;
        IEntryLikeCacheRepository entryLikeCacheRepo;

        public EntryLikeProvider() : this(
            new EntryLikeRepository(),
            new RedisEntryLikeRepository())
        {

        }
        public EntryLikeProvider(
            IEntryLikeRepository entryLikeRepo,
            IEntryLikeCacheRepository entryLikeCacheRepo)
        {
            this.entryLikeRepo = entryLikeRepo;
            this.entryLikeCacheRepo = entryLikeCacheRepo;
        }
        public void LikeComment(long commentId, string userId)
        {
            var success = entryLikeRepo.LikeComment(commentId, userId);
            if (success)
            {
                try
                {
                    entryLikeCacheRepo.IncrementCommentLikeCount(commentId);
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format(
                        "[LikeComment] Error incrementing comment like count for comment: {0}, By user : {1}", commentId, userId), ex);
                }
            }
        }
        public void UnlikeComment(long commentId, string userId)
        {
            var success = entryLikeRepo.UnlikeComment(commentId, userId);
            if (success)
            {
                try
                {
                    entryLikeCacheRepo.DecrementCommentLikeCount(commentId);
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format(
                        "[UnlikeComment] Error decrementing comment like count for comment: {0}, By user : {1}", commentId, userId), ex);
                }
            }
        }

        public void LikePost(string postId, string userId)
        {
            var success = entryLikeRepo.LikePost(postId, userId);
            if (success)
            {
                try
                {
                    entryLikeCacheRepo.IncrementPostLikeCount(postId);
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format(
                        "[LikePost] Error incrmenting post like count for post: {0}, By user : {1}", postId, userId), ex);
                }
            }
        }


        public void UnlikePost(string postId, string userId)
        {
            var success = entryLikeRepo.UnlikePost(postId, userId);
            if (success)
            {
                try
                {
                    entryLikeCacheRepo.DecrementPostLikeCount(postId);
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format(
                        "[UnlikePost] Error decrementing post like count for post: {0}, By user : {1}", postId, userId), ex);
                }
            }
        }

        public IEnumerable<string> GetPostLikes(string postId)
        {
            return entryLikeRepo.GetPostLikes(postId);
        }

        public IEnumerable<string> GetCommentLikes(long commentId)
        {
            return entryLikeRepo.GetCommentLikes(commentId);
        }

        public int GetPostLikeCountCached(string postId)
        {
            return entryLikeCacheRepo.GetPostLikeCount(postId);
        }

        public int GetCommentLikeCountCached(long commentId)
        {
            return entryLikeCacheRepo.GetCommentLikeCount(commentId);
        }

        public EntryLikePagedModel GetPostLikesPaged(string postId, int skip, int size)
        {
            return entryLikeRepo.GetPostLikesPaged(postId, skip, size);
        }

        public EntryLikePagedModel GetCommentLikesPaged(long commentId, int skip, int size)
        {
            return entryLikeRepo.GetCommentLikesPaged(commentId, skip, size);
        }
    }
}
