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
        #region CRUD

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
            using (var entities = new SocialFeedEntities())
            {
                var result = entities.WallPost.Include("UserWall").Include("GroupWall").FirstOrDefault(p => p.Id == postId && p.IsDeleted == false);
                return MapWallPost(result);
            }
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

        public WallPostWithDetailsModel GetPostDetailed(string postId)
        {
            GetWallPost_Result postResult;
            IEnumerable<GetLatestComments_Result> commentResult;
            using (var entities = new SocialFeedEntities())
            {
                postResult = entities.GetWallPost(postId).FirstOrDefault();
                if (postResult != null)
                {
                    commentResult = entities.GetLatestComments(postId).ToList();
                    return MapWallPost(postResult, commentResult);
                }
            }
            return null;
        }

        #endregion

        #region Wall

        public IEnumerable<WallPostModel> GetUserWall(string userId, DateTime olderThan, int size)
        {
            List<UserWall> posts;
            using (var entities = new SocialFeedEntities())
            {
                posts = entities.UserWall.Include("WallPost")
                    .Where(p => p.UserId == userId && p.WallPost.CreatedDate < olderThan && p.WallPost.IsDeleted == false)
                    .OrderByDescending(p => p.WallPost.CreatedDate).Take(size).ToList();
            }

            foreach (var post in posts)
            {
                yield return MapWallPost(post.WallPost);
            }
        }

        public IEnumerable<WallPostWithDetailsModel> GetUserWallDetailed(string userId, DateTime olderThan, int size)
        {
            IEnumerable<GetWall_Result> procedureResult;
            using (var entities = new SocialFeedEntities())
            {
                procedureResult = entities.GetUserWall(userId, olderThan, size).ToList();
            }
            return MapWall(procedureResult, new WallModel { OwnerId = userId, WallOwnerType = WallType.user });
        }

        public IEnumerable<WallPostModel> GetGroupWall(string groupId, DateTime olderThan, int size)
        {
            List<GroupWall> posts;
            using (var entities = new SocialFeedEntities())
            {
                posts = entities.GroupWall.Include("WallPost")
                    .Where(p => p.GroupId == groupId && p.WallPost.CreatedDate < olderThan && p.WallPost.IsDeleted == false)
                    .OrderByDescending(p => p.WallPost.CreatedDate).Take(size).ToList();
            }

            foreach (var post in posts)
            {
                yield return MapWallPost(post.WallPost);
            }
        }

        public IEnumerable<WallPostWithDetailsModel> GetGroupWallDetailed(string groupId, DateTime olderThan, int size)
        {
            IEnumerable<GetWall_Result> procedureResult;
            using (var entities = new SocialFeedEntities())
            {
                procedureResult = entities.GetGroupWall(groupId, olderThan, size).ToList();
            }
            return MapWall(procedureResult, new WallModel { OwnerId = groupId, WallOwnerType = WallType.group });
        }

        #endregion

        #region Mapping

        private WallPostModel MapWallPost(WallPost model)
        {
            if (model != null)
            {
                var result = new WallPostModel
                {
                    Body = model.Body,
                    CreatedDate = model.CreatedDate,
                    Id = model.Id,
                    ModifiedDate = model.ModifiedDate,
                    PostedBy = model.CreatedBy,
                    PostType = model.PostType
                };
                if (model.UserWall != null)
                {
                    result.WallOwner = new WallModel { OwnerId = model.UserWall.UserId, WallOwnerType = WallType.user };
                }
                else if (model.GroupWall != null)
                {
                    result.WallOwner = new WallModel { OwnerId = model.GroupWall.GroupId, WallOwnerType = WallType.group };
                }
                return result;
            }
            return null;
        }

        private WallPostWithDetailsModel MapWallPost(GetWallPost_Result postResult,
          IEnumerable<GetLatestComments_Result> commentResult)
        {

            var mapped = new WallPostWithDetailsModel
            {
                Body = postResult.Body,
                CommentCount = postResult.CommentCount.Value,
                CreatedDate = postResult.CreatedDate,
                Id = postResult.Id,
                LikeCount = postResult.LikeCount.Value,
                ModifiedDate = postResult.ModifiedDate,
                PostedBy = postResult.CreatedBy,
                PostType = postResult.PostType
            };
            var wallModel = new WallModel();
            if (!string.IsNullOrWhiteSpace(postResult.GroupId))
            {
                wallModel.OwnerId = postResult.GroupId;
                wallModel.WallOwnerType = WallType.group;
            }
            else
            {
                wallModel.OwnerId = postResult.UserId;
                wallModel.WallOwnerType = WallType.user;
            }
            mapped.WallOwner = wallModel;

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
            mapped.LatestComments = commentList;
            return mapped;
        }

        private IEnumerable<WallPostWithDetailsModel> MapWall(IEnumerable<GetWall_Result> procedureResult, WallModel wallOwner)
        {
            var result = new List<WallPostWithDetailsModel>();
            if (procedureResult != null)
            {
                foreach (var record in procedureResult)
                {
                    var comment = MapComment(record);
                    var existingRecord = result.FirstOrDefault(p => p.Id == record.Id);
                    if (existingRecord == null)
                    {
                        var relatedPost = new WallPostWithDetailsModel
                        {
                            Body = record.Body,
                            CommentCount = record.CommentCount.Value,
                            CreatedDate = record.CreatedDate,
                            Id = record.Id,
                            LikeCount = record.LikeCount.Value,
                            ModifiedDate = record.ModifiedDate,
                            PostedBy = record.CreatedBy,
                            PostType = record.PostType,
                            WallOwner = wallOwner,
                            LatestComments = new List<CommentModel>()
                        };
                        existingRecord = relatedPost;
                        result.Add(relatedPost);
                    }
                    if (comment != null)
                    {
                        (existingRecord.LatestComments as List<CommentModel>).Add(comment);
                    }
                }
            }
            return result;
        }

        private CommentModel MapComment(GetWall_Result record)
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

        #endregion

    }
}

