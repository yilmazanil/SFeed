using System;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Comments;
using SFeed.RedisRepository.Implementation;
using SFeed.SqlRepository.Implementation;
using SFeed.Core.Infrastructure.Repository;
using SFeed.Core.Infrastructure.Caching;
using log4net;

namespace SFeed.Business.Providers
{
    public class CommentProvider : ICommentProvider
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(WallPostProvider));

        ICommentRepository commentRepo;
        ICommentCountCacheRepository commentCounterCacheRepo;
        IEntryLikeCacheRepository entryLikeCacheRepo;

        public CommentProvider() :
            this(new CommentRepository(),
                new RedisCommentCountRepository(),
            new RedisEntryLikeRepository())
        {

        }
        public CommentProvider(ICommentRepository commentRepo,
              ICommentCountCacheRepository commentCounterCacheRepo,
              IEntryLikeCacheRepository entryLikeCacheRepo)
        {
            this.commentRepo = commentRepo;
            this.commentCounterCacheRepo = commentCounterCacheRepo;
            this.entryLikeCacheRepo = entryLikeCacheRepo;
        }

        public long AddComment(CommentCreateRequest request)
        {
            var result = commentRepo.SaveComment(request);
            if (result != null && result.Id > 0)
            {
                var cacheModel = new CommentCacheModel
                {
                    Body = request.Body,
                    CommentId = result.Id,
                    CreatedBy = request.CreatedBy,
                    CreatedDate = result.CreatedDate,
                    PostId = request.WallPostId
                };
                try
                {
                    commentCounterCacheRepo.Increment(cacheModel.PostId);
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format(
                    "[AddComment] Error incrementing comment count for postId : {0}", cacheModel.PostId), ex);
                }

                return result.Id;
            }
            return 0;

        }

        public void UpdateComment(CommentUpdateRequest request)
        {
            commentRepo.UpdateComment(request);
        }

        public void DeleteComment(string postId, long commentId)
        {
            commentRepo.RemoveComment(commentId);
            try
            {
                commentCounterCacheRepo.Remove(postId);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("[DeleteComment] Error removing comment counter for postId: {0}", postId), ex);
            }
        }

        public IEnumerable<CommentModel> GetComments(string postId, int skip, int size)
        {
             return commentRepo.GetPagedComments(postId, skip, size);
        }

        public IEnumerable<CommentModel> GetLatestComments(string postId)
        {
            return commentRepo.GetPagedComments(postId, 0, 3);
        }

        public CommentModel GetComment(string postId, long commentId)
        {
            return commentRepo.GetComment(postId, commentId);
        }
    }
}
