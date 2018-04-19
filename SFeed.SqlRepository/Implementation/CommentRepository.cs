using System;
using System.Collections.Generic;
using System.Linq;
using SFeed.Core.Models.Comments;
using SFeed.Core.Infrastructure.Repository;

namespace SFeed.SqlRepository.Implementation
{
    public class CommentRepository : ICommentRepository
    {
        public CommentCreateResponse SaveComment(CommentCreateRequest model)
        {
            var entry = new UserComment
            {
                WallPostId = model.WallPostId,
                Body = model.Body,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };
            using (var entities = new SocialFeedEntities())
            {
                entities.UserComment.Add(entry);
                entities.SaveChanges();
            }

            return new CommentCreateResponse { Id = entry.Id, CreatedDate = entry.CreatedDate };
        }

        public DateTime? UpdateComment(CommentUpdateRequest model)
        {
            using (var entities = new SocialFeedEntities())
            {
                var comment = entities.UserComment.FirstOrDefault(p => p.Id == model.CommentId &&
                    p.IsDeleted == false);
                if (comment != null)
                {
                    comment.ModifiedDate = DateTime.Now;
                    comment.Body = model.Body;
                    entities.SaveChanges();
                    return comment.ModifiedDate.Value;
                }
            }
            return null;
        }

        public CommentModel GetComment(string postId, long commentId)
        {
            using (var context = new SocialFeedEntities())
            {
                var result = context.UserComment.FirstOrDefault(t => t.WallPostId == postId && t.Id == commentId && t.IsDeleted == false);
                if (result != null)
                {
                    var likeCount = context.UserCommentLike.Count(t => t.CommentId == commentId);
                    return MapCommentWithDetails(result, likeCount);
                }
            }
            return null;
        }

        public void RemoveComment(long commentId)
        {
            using (var entities = new SocialFeedEntities())
            {
                var comment = entities.UserComment.FirstOrDefault(p => p.Id == commentId);
                comment.IsDeleted = true;
                entities.SaveChanges();
            }
        }

        public IEnumerable<CommentModel> GetPagedComments(string postId, int skip, int size)
        {
            var result = new List<CommentModel>();
            using (var context = new SocialFeedEntities())
            {
                var comments = context.GetComments(postId, skip, size).ToList();
                return MapCommentProcedureResult(comments);
            }
        }

        #region Mapping
        private CommentModel MapComment(UserComment userComment)
        {
            var result = new CommentModel()
            {
                Body = userComment.Body,
                CreatedBy = userComment.CreatedBy,
                CreatedDate = userComment.CreatedDate,
                ModifiedDate = userComment.ModifiedDate,
                Id = userComment.Id,
            };
            return result;
        }

        private CommentModel MapCommentWithDetails(UserComment userComment, int likeCount)
        {
            var result = new CommentModel()
            {
                Body = userComment.Body,
                CreatedBy = userComment.CreatedBy,
                CreatedDate = userComment.CreatedDate,
                ModifiedDate = userComment.ModifiedDate,
                Id = userComment.Id,
                LikeCount = likeCount
            };
            return result;
        }

        private IEnumerable<CommentModel> MapCommentProcedureResult(IEnumerable<GetComments_Result> procedureResult)
        {
            var returnList = new List<CommentModel>();
            foreach (var comment in procedureResult)
            {
                var model = new CommentModel
                {
                    Body = comment.Body,
                    CreatedBy = comment.CreatedBy,
                    CreatedDate = comment.CreatedDate,
                    Id = comment.Id,
                    LikeCount = comment.LikeCount.Value,
                    ModifiedDate = comment.ModifiedDate
                };
                returnList.Add(model);
            }
            return returnList;
        }
        #endregion
    }
}
