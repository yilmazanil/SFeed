using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Infrastructure.Repository.Caching;
using SFeed.Core.Infrastructure.Repository.Sql;
using SFeed.RedisRepository.Implementation;
using SFeed.SqlRepository.Implementation;
using System.Collections.Generic;

namespace SFeed.Business.Providers
{
    public class EntryLikeProvider : IEntryLikeProvider
    {
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
                entryLikeCacheRepo.IncrementCommentLikeCount(commentId);
            }
        }
        public void UnlikeComment(long commentId, string userId)
        {
            var success = entryLikeRepo.UnlikeComment(commentId, userId);
            if (success)
            {
                entryLikeCacheRepo.DecrementCommentLikeCount(commentId);
            }
        }

        public void LikePost(string postId, string userId)
        {
            var success  = entryLikeRepo.LikePost(postId, userId);
            if (success)
            {
                entryLikeCacheRepo.IncrementPostLikeCount(postId);
            }
        }


        public void UnlikePost(string postId, string userId)
        {
            var success = entryLikeRepo.UnlikePost(postId, userId);
            if (success)
            {
                entryLikeCacheRepo.DecrementPostLikeCount(postId);
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

    }
}
