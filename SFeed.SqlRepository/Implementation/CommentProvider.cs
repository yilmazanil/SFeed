using SFeed.Core.Infrastructure.Repository.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using SFeed.Core.Models.Comments;

namespace SFeed.SqlRepository.Implementation
{
    public class CommentRepository : ICommentRepository
    {
        public CommentModel GetComment(string postId, long commentId)
        {
            using (var context = new SocialFeedEntities())
            {
                var result = context.UserComment.FirstOrDefault(t => t.WallPostId == postId && t.Id == commentId && t.IsDeleted == false);
                if (result != null)
                {
                    var likeCount = context.UserCommentLike.Count(t => t.CommentId == commentId);
                    return MapDbEntry(result, likeCount);
                }
            }
            return null;
        }

        public IEnumerable<CommentModel> GetComments(string postId, DateTime olderThan, int size)
        {
            var result = new List<CommentModel>();
            using (var context = new SocialFeedEntities())
            {
                var comments = context.UserComment.Where(t => t.WallPostId == postId && t.CreatedDate< olderThan && t.IsDeleted == false)
                    .OrderByDescending(p=>p.Id).Take(size).ToList();
                foreach (var comment in comments)
                {
                    var likeCount = context.UserCommentLike.Count(t => t.CommentId == comment.Id);
                    result.Add(MapDbEntry(comment, likeCount));
                }

            }
            return result;
        }

        public void RemoveItem(string postId, long commentId)
        {
            using (var entities = new SocialFeedEntities())
            {
                var comment = entities.UserComment.FirstOrDefault(p => p.WallPostId == postId && p.Id == commentId);
                comment.IsDeleted = true;
                entities.SaveChanges();
            }
        }

        public CommentCreateResponse SaveItem(CommentCreateRequest model)
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

            return new CommentCreateResponse { Id = entry.Id , CreatedDate = entry.CreatedDate };
        }

        public DateTime? UpdateItem(CommentUpdateRequest model)
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

        private CommentModel MapDbEntry(UserComment userComment, int likeCount)
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
    }
}
