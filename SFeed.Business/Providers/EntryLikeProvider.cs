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
        ILikeCountCacheRepository entryLikeCacheRepo;

        public EntryLikeProvider() : this(
            new EntryLikeRepository(),
            new RedisEntryLikeRepository())
        {

        }
        public EntryLikeProvider(
            IEntryLikeRepository entryLikeRepo,
            ILikeCountCacheRepository entryLikeCacheRepo)
        {
            this.entryLikeRepo = entryLikeRepo;
            this.entryLikeCacheRepo = entryLikeCacheRepo;
        }
        public void LikeComment(long commentId, string userId)
        {
             entryLikeRepo.LikeComment(commentId, userId);
        }
        public void UnlikeComment(long commentId, string userId)
        {
            entryLikeRepo.UnlikeComment(commentId, userId);
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
