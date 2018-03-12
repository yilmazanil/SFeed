using System;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Comments;
using SFeed.Core.Models.Caching;
using SFeed.Core.Infrastructure.Repository.Caching;
using SFeed.Core.Infrastructure.Repository.Sql;
using SFeed.RedisRepository.Implementation;
using SFeed.SqlRepository.Implementation;

namespace SFeed.Business.Providers
{
    public class UserCommentProvider : IUserCommentProvider
    {
        ICommentRepository commentRepo;
        ICommentCacheRepository commentCacheRepo;

        public UserCommentProvider() : 
            this(new CommentRepository(),
            new RedisCommentRepository())
        {

        }
        public UserCommentProvider(ICommentRepository commentRepo,
              ICommentCacheRepository commentCacheRepo)
        {
            this.commentRepo = commentRepo;
            this.commentCacheRepo = commentCacheRepo;
        }

        public long AddComment(CommentCreateRequest request)
        {
            var result = commentRepo.SaveItem(request);
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

                commentCacheRepo.AddItem(cacheModel);

                return result.Id;
            }
            return 0;

        }

        public void UpdateComment(CommentUpdateRequest request)
        {
            var result = commentRepo.UpdateItem(request);
            if (result.HasValue)
            {
                commentCacheRepo.UpdateItem(request, result.Value);
            }
        }

        public IEnumerable<CommentModel> GetComments(string postId, DateTime olderThan, int size)
        {
            return commentRepo.GetComments(postId, olderThan, size);
        }

        public void DeleteComment(string postId, long commentId)
        {
            commentRepo.RemoveItem(postId, commentId);
            commentCacheRepo.RemoveComment(postId, commentId);
        }

        public CommentModel GetComment(string postId, long commentId)
        {
           return commentRepo.GetComment(postId, commentId);
        }
    }
}
