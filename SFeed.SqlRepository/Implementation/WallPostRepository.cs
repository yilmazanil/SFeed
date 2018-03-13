using System;
using SFeed.Core.Models.WallPost;
using System.Linq;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Repository;
using SFeed.Core.Models.Wall;
using SFeed.Core.Models.Comments;

namespace SFeed.SqlRepository.Implementation
{
    public class WallPostRepository : IWallPostRepository
    {
        public WallPostCreateResponse SavePost(WallPostCreateRequest model)
        {
            var newEntryId = Guid.NewGuid().ToString();
            var entry = new WallPost
            {
                Body = model.Body,
                CreatedBy = model.PostedBy,
                Id = newEntryId,
                CreatedDate = DateTime.Now,
                PostType = (byte)model.PostType,
                IsDeleted = false
            };

            if (model.TargetWall.WallOwnerType == WallType.user)
            {
                entry.UserWall = new UserWall { UserId = model.TargetWall.OwnerId, WallPostId = newEntryId };
            }
            else
            {
                entry.GroupWall = new GroupWall { GroupId = model.TargetWall.OwnerId, WallPostId = newEntryId };
            }

            using (var entities = new SocialFeedEntities())
            {
                entities.WallPost.Add(entry);
                entities.SaveChanges();
            }

            return new WallPostCreateResponse { PostId = newEntryId, CreatedDate = entry.CreatedDate };
        }

        public DateTime? UpdatePost(WallPostUpdateRequest model)
        {
            using (var entities = new SocialFeedEntities())
            {
                var post = entities.WallPost.FirstOrDefault(p => p.Id == model.PostId && p.IsDeleted == false);
                if (post != null)
                {
                    post.Body = model.Body;
                    post.ModifiedDate = DateTime.Now;
                    post.PostType = (byte)model.PostType;
                    entities.SaveChanges();
                    return post.ModifiedDate.Value;
                }
            }
            return null;
        }

        public WallPostModel GetPost(string postId)
        {
            GetWallPost_Result postResult;
            IEnumerable<GetLatestComments_Result> commentResult;
            using (var entities = new SocialFeedEntities())
            {
                postResult = entities.GetWallPost(postId).FirstOrDefault();
                if (postResult != null)
                {
                    commentResult = entities.GetLatestComments(postId).ToList();
                }
                else
                {
                    return null;
                }
            }
            return MapGetPostProcedureResult(postResult, commentResult);

        }

        private WallPostModel MapGetPostProcedureResult(GetWallPost_Result postResult, IEnumerable<GetLatestComments_Result> commentResult)
        {
            var relatedPost = new WallPostModel
            {
                Body = postResult.Body,
                CommentCount = postResult.CommentCount.Value,
                CreatedDate = postResult.CreatedDate,
                Id = postResult.Id,
                LikeCount = postResult.LikeCount.Value,
                ModifiedDate = postResult.ModifiedDate,
                PostedBy = postResult.CreatedBy,
                PostType = postResult.PostType,
                WallOwner = !string.IsNullOrWhiteSpace(postResult.GroupId) ? new WallModel
                {
                    IsPublic = true,
                    OwnerId = postResult.GroupId,
                    WallOwnerType = WallType.group
                } : new WallModel
                {
                    IsPublic = true,
                    OwnerId = postResult.UserId,
                    WallOwnerType = WallType.user
                }
            };
            var commentList = new List<CommentModel>();
            if (commentResult != null && commentResult.Any())
            {
                foreach (var comment in commentResult)
                {
                    commentList.Add(new CommentModel
                    {
                        Body = comment.Body,
                        CreatedBy = comment.CreatedBy,
                        CreatedDate = comment.CreatedDate,
                        Id = comment.Id,
                        LikeCount = comment.LikeCount.Value,
                        ModifiedDate = comment.ModifiedDate
                    });
                }
            }
            relatedPost.LatestComments = commentList;
            return relatedPost;
        }

        public bool RemovePost(string postId)
        {
            using (var entities = new SocialFeedEntities())
            {
                var post = entities.WallPost.FirstOrDefault(p => p.Id == postId);
                if (post != null)
                {
                    post.IsDeleted = true;
                    entities.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public IEnumerable<string> GetUserPostIds(string userId)
        {
            using (var entities = new SocialFeedEntities())
            {
                return entities.WallPost.Where(p => p.CreatedBy == userId && p.IsDeleted == false).Select(p => p.Id).ToList();
            }
        }

        public IEnumerable<WallPostModel> GetUserWall(string userId, DateTime olderThan, int size)
        {
            IEnumerable<GetWall_Result> userWallEntries;
            using (var entities = new SocialFeedEntities())
            {
                userWallEntries = entities.GetUserWall(userId, olderThan, size);
            }
            return MapWallResult(userWallEntries, new WallModel { IsPublic = true, OwnerId = userId, WallOwnerType = WallType.user  });
        }

        public IEnumerable<WallPostModel> GetGroupWall(string groupId, DateTime olderThan, int size)
        {

            IEnumerable<GetWall_Result> userWallEntries;
            using (var entities = new SocialFeedEntities())
            {
                userWallEntries = entities.GetGroupWall(groupId, olderThan, size);
            }
            return MapWallResult(userWallEntries, new WallModel { IsPublic = true, OwnerId = groupId, WallOwnerType = WallType.group });
        }

        private IEnumerable<WallPostModel> MapWallResult(IEnumerable<GetWall_Result> wallPostResult, WallModel wallOwner)
        {
            List<WallPostModel> result = new List<WallPostModel>();
            if (wallPostResult != null && wallPostResult.Any())
            {
                foreach (var record in wallPostResult)
                {
                    var existingRecord = result.FirstOrDefault(p => p.Id == record.Id);
                    if (existingRecord == null)
                    {
                        var relatedPost = new WallPostModel
                        {
                            Body = record.Body,
                            CommentCount = record.CommentCount.Value,
                            CreatedDate = record.CreatedDate,
                            Id = record.Id,
                            LikeCount = record.LikeCount.Value,
                            ModifiedDate = record.ModifiedDate,
                            PostedBy = record.CreatedBy,
                            PostType = record.PostType,
                            WallOwner = wallOwner
                        };

                        relatedPost.LatestComments = new List<CommentModel>();
                        result.Add(relatedPost);

                    }
                    existingRecord = result.FirstOrDefault(p => p.Id == record.Id);

                    var comment = MapWallResultToComment(record);
                    if (comment != null)
                    {
                        (existingRecord.LatestComments as List<CommentModel>).Add(comment);
                    }
                }
            }
            return result;
        }

        private CommentModel MapWallResultToComment(GetWall_Result record)
        {
            if (record.CommentId.HasValue)
            {
                var comment = new CommentModel
                {
                    Body = record.CommentBody,
                    CreatedBy = record.CommentCreatedBy,
                    CreatedDate = record.CommentCreatedDate.Value,
                    Id = record.CommentId.Value,
                    LikeCount = record.CommentLikeCount.Value,
                    ModifiedDate = record.CommentModifiedDate
                };
                return comment;
            }
            return null;
        }
    }
}

