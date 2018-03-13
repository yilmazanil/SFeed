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
            List<WallPost> dbEntries;
            using (var entities = new SocialFeedEntities())
            {
                dbEntries = entities.WallPost.Include("UserWall").Include("GroupWall").Where(
                    p => p.CreatedDate < olderThan && p.IsDeleted == false
                    && p.UserWall.UserId == userId).OrderByDescending(p => p.Id).Take(size).ToList();
            }

            var returnList = new List<WallPostModel>();
            foreach (var entry in dbEntries)
            {
                returnList.Add(MapDbEntity(entry));
            }
            return returnList;
        }

        public IEnumerable<WallPostModel> GetGroupWall(string groupId, DateTime olderThan, int size)
        {
            List<WallPost> dbEntries;
            using (var entities = new SocialFeedEntities())
            {
                dbEntries = entities.WallPost.Include("UserWall").Include("GroupWall").Where(
                    p => p.CreatedDate < olderThan && p.IsDeleted == false
                    && p.GroupWall.GroupId == groupId).Take(size).ToList();
            }

            var returnList = new List<WallPostModel>();
            foreach (var entry in dbEntries)
            {
                returnList.Add(MapDbEntity(entry));
            }
            return returnList;
        }



        private WallPostModel MapDbEntity(WallPost entry)
        {
            if (entry != null)
            {
                var retVal = new WallPostModel
                {
                    Body = entry.Body,
                    Id = entry.Id,
                    PostedBy = entry.CreatedBy,
                    PostType = entry.PostType,
                    ModifiedDate = entry.ModifiedDate,
                    CreatedDate = entry.CreatedDate

                };

                if (entry.UserWall != null)
                {
                    retVal.WallOwner = new WallModel { WallOwnerType = WallType.user, OwnerId = entry.UserWall.UserId };
                }
                else
                {
                    retVal.WallOwner = new WallModel { WallOwnerType = WallType.group, OwnerId = entry.GroupWall.GroupId };
                }
                return retVal;
            }
            return null;
        }
    }
}
