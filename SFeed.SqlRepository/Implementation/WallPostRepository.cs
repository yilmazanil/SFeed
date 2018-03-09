using System;
using SFeed.Core.Infrastructure.Repository.Sql;
using SFeed.Core.Models.WallPost;
using SFeed.Core.Models;
using System.Linq;
using System.Collections.Generic;

namespace SFeed.SqlRepository.Implementation
{
    public class WallPostRepository : IWallPostRepository
    {
        public WallPostCreateResponse SaveItem(WallPostCreateRequest model)
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

            if (model.WallOwner.WallOwnerType == WallOwnerType.user)
            {
                entry.UserWall = new UserWall { UserId = model.WallOwner.Id, WallPostId = newEntryId };
            }
            else
            {
                entry.GroupWall = new GroupWall { GroupId = model.WallOwner.Id, WallPostId = newEntryId };
            }

            using (var entities = new SocialFeedEntities())
            {
                entities.WallPost.Add(entry);
                entities.SaveChanges();
            }

            return new WallPostCreateResponse { PostId = newEntryId, CreatedDate = entry.CreatedDate };
        }

        public WallPostModel GetItem(string postId)
        {
            WallPost relatedPost;
            using (var entities = new SocialFeedEntities())
            {
                relatedPost = entities.WallPost.Include("UserWall").Include("GroupWall").FirstOrDefault(t => t.Id == postId && t.IsDeleted == false);
            }
            if (relatedPost != null)
            {
                return MapDbEntity(relatedPost);
            }
            return null;
        }

        public bool RemoveItem(string postId)
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
                    retVal.WallOwner = new WallOwner { WallOwnerType = WallOwnerType.user, Id = entry.UserWall.UserId };
                }
                else
                {
                    retVal.WallOwner = new WallOwner { WallOwnerType = WallOwnerType.group, Id = entry.GroupWall.GroupId };
                }
                return retVal;
            }
            return null;
        }

        public DateTime? UpdateItem(WallPostUpdateRequest model)
        {
            using (var entities = new SocialFeedEntities())
            {
                var post = entities.WallPost.FirstOrDefault(p => p.Id == model.PostId && p.IsDeleted == false);
                if (post != null)
                {
                    post.ModifiedDate = DateTime.Now;
                    post.PostType = (byte)model.PostType;
                    entities.SaveChanges();
                    return post.ModifiedDate.Value;
                }
            }
            return null;
        }

        public IEnumerable<WallPostModel> GetUserWall(WallOwner wallOwner, DateTime olderThan, int size)
        {
            List<WallPost> dbEntries;
            using (var entities = new SocialFeedEntities())
            {
                dbEntries = entities.WallPost.Where(p => p.CreatedDate> olderThan && p.IsDeleted == false).Take(size).ToList();
            }

            foreach (var entry in dbEntries)
            {
                yield return MapDbEntity(entry);
            }
        }
    }
}
