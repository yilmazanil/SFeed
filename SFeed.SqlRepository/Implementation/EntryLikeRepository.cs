using System;
using System.Linq;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Repository;
using SFeed.Core.Models.EntryLike;

namespace SFeed.SqlRepository.Implementation
{
    public class EntryLikeRepository : IEntryLikeRepository
    {
        public bool LikeComment(long commentId, string userId)
        {
            using (var entities = new SocialFeedEntities())
            {
                if (!entities.UserCommentLike.Any(t => t.CreatedBy == userId && t.CommentId == commentId))
                {
                    entities.UserCommentLike.Add(new UserCommentLike {
                        CreatedDate = DateTime.Now, CommentId = commentId , CreatedBy = userId });
                    entities.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public bool UnlikeComment(long commentId, string userId)
        {
            using (var entities = new SocialFeedEntities())
            {
                var likeEntry = entities.UserCommentLike.FirstOrDefault(t => t.CreatedBy == userId && t.CommentId == commentId);

                if (likeEntry != null)
                {
                    entities.UserCommentLike.Remove(likeEntry);
                    entities.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool LikePost(string postId, string userId)
        {
            using (var entities = new SocialFeedEntities())
            {
                if (!entities.WallPostLike.Any(t => t.CreatedBy == userId && t.WallPostId == postId))
                {
                    entities.WallPostLike.Add(new WallPostLike { CreatedDate = DateTime.Now, WallPostId = postId, CreatedBy = userId });
                    entities.SaveChanges();
                    return true;
                }
                return false;
            }
           
        }
        public bool UnlikePost(string postId, string userId)
        {
            using (var entities = new SocialFeedEntities())
            {
                var likeEntry = entities.WallPostLike.FirstOrDefault(t => t.CreatedBy == userId && t.WallPostId == postId);

                if (likeEntry != null)
                {
                    entities.WallPostLike.Remove(likeEntry);
                    entities.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public IEnumerable<string> GetPostLikes(string postId)
        {
            using (var entities = new SocialFeedEntities())
            {
                return entities.WallPostLike.Where(t => t.WallPostId == postId).Select(t => t.CreatedBy).ToList();
            }
        }

        public IEnumerable<string> GetCommentLikes(long commentId)
        {
            using (var entities = new SocialFeedEntities())
            {
                return entities.UserCommentLike.Where(t => t.CommentId == commentId).Select(t => t.CreatedBy).ToList();
            }
        }

        public EntryLikePagedModel GetPostLikesPaged(string postId, int skip, int size)
        {
            using (var entities = new SocialFeedEntities())
            {
                var totalCount = entities.WallPostLike.Where(p => p.WallPostId == postId).Count();

                var records = entities.WallPostLike.Where(p => p.WallPostId == postId)
                    .OrderByDescending(p => p.CreatedDate)
                    .Skip(skip).Take(size).Select(p=>p.CreatedBy).ToList();

                return new EntryLikePagedModel
                {
                    Records = records,
                    TotalCount = totalCount
                };
            }
        }

        public EntryLikePagedModel GetCommentLikesPaged(long commentId, int skip, int size)
        {
            using (var entities = new SocialFeedEntities())
            {
                var totalCount = entities.UserCommentLike.Where(p => p.CommentId == commentId).Count();

                var records = entities.UserCommentLike.Where(p => p.CommentId == commentId)
                    .OrderByDescending(p => p.CreatedDate)
                    .Skip(skip).Take(size).Select(p => p.CreatedBy).ToList();

                return new EntryLikePagedModel
                {
                    Records = records,
                    TotalCount = totalCount
                };
            }
        }
    }
}
