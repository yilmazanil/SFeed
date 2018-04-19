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
    public class UserCommentProvider : ICommentProvider
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(WallPostProvider));

        ICommentRepository commentRepo;
        ICommentCacheRepository commentCacheRepo;
        IEntryLikeCacheRepository entryLikeCacheRepo;

        public UserCommentProvider() :
            this(new CommentRepository(),
            new RedisCommentRepository(),
            new RedisEntryLikeRepository())
        {

        }
        public UserCommentProvider(ICommentRepository commentRepo,
              ICommentCacheRepository commentCacheRepo,
              IEntryLikeCacheRepository entryLikeCacheRepo)
        {
            this.commentRepo = commentRepo;
            this.commentCacheRepo = commentCacheRepo;
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
                    commentCacheRepo.AddComment(cacheModel);
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format(
                    "[AddComment] Error updating cache for comment: {0}", result.Id), ex);
                }

                return result.Id;
            }
            return 0;

        }

        public void UpdateComment(CommentUpdateRequest request)
        {
            var result = commentRepo.UpdateComment(request);
            if (result.HasValue)
            {
                try
                {
                    commentCacheRepo.UpdateComment(request, result.Value);
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format(
                    "[UpdateComment] Error updating cache for comment: {0}", request.CommentId), ex);
                }
            }
        }

        public void DeleteComment(string postId, long commentId)
        {
            commentRepo.RemoveComment(commentId);
            try
            {
                commentCacheRepo.RemoveComment(postId, commentId);
            }
            catch (Exception ex)
            {
                logger.Error(string.Format(
                "[DeleteComment] Error updating cache for comment: {0}", commentId), ex);
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
