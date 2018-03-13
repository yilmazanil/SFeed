using System.Linq;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Repository;
using SFeed.Core.Models.Follower;
using SFeed.Core.Models;
using SFeed.Core.Models.Wall;
using System;

namespace SFeed.SqlRepository.Implementation
{
    public class FollowerRepository : IFollowerRepository
    {
        public void FollowGroup(string groupId, string followerId)
        {
            using (var entities = new SocialFeedEntities())
            {
                if (!entities.GroupFollower.Any(t => t.GroupId == groupId && t.FollowerId == followerId))
                {
                    entities.GroupFollower.Add(new GroupFollower { GroupId = groupId, FollowerId = followerId });
                    entities.SaveChanges();
                }
            }
        }

        public void FollowUser(string userId, string followerId)
        {
            using (var entities = new SocialFeedEntities())
            {
                if (!entities.UserFollower.Any(t => t.UserId == userId && t.FollowerId == followerId))
                {
                    entities.UserFollower.Add(new UserFollower { UserId = userId, FollowerId = followerId });
                    entities.SaveChanges();
                }
            }
        }

        public void UnfollowGroup(string groupId, string followerId)
        {
            using (var entities = new SocialFeedEntities())
            {
                var record = entities.GroupFollower.FirstOrDefault(p => p.FollowerId == followerId && p.GroupId == groupId);
                if (record != null)
                {
                    entities.GroupFollower.Remove(record);
                    entities.SaveChanges();
                }
            }
        }

        public void UnfollowUser(string userId, string followerId)
        {
            using (var entities = new SocialFeedEntities())
            {
                var record = entities.UserFollower.FirstOrDefault(p => p.FollowerId == followerId && p.UserId == userId);
                if (record != null)
                {
                    entities.UserFollower.Remove(record);
                    entities.SaveChanges();
                }
            }
        }

        public IEnumerable<string> GetFollowersUser(string userId)
        {
            using (var entities = new SocialFeedEntities())
            {
                return entities.UserFollower.Where(t => t.UserId == userId).Select(t => t.FollowerId).ToList();
            }
        }

        public FollowerPagedModel GetFollowersUserPaged(string userId, int skip, int size)
        {
            using (var entities = new SocialFeedEntities())
            {
                var followers = entities.UserFollower.Where(p => p.UserId == userId).Select(p => p.FollowerId);
                var resultSet = followers.OrderBy(p => p).Skip(skip).Take(size).ToList();
                var totalCount = followers.Count();

                return new FollowerPagedModel
                {
                    Records = resultSet,
                    TotalCount = totalCount
                };
            }
        }

        public IEnumerable<string> GetFollowersGroup(string groupId)
        {
            using (var entities = new SocialFeedEntities())
            {
                return entities.GroupFollower.Where(t => t.GroupId == groupId).Select(t => t.FollowerId).ToList();
            }
        }

        public FollowerPagedModel GetFollowersGroupPaged(string groupId, int skip, int size)
        {
            using (var entities = new SocialFeedEntities())
            {
                var followers = entities.GroupFollower.Where(p => p.GroupId == groupId).Select(p => p.FollowerId);
                var resultSet = followers.OrderBy(p => p).Skip(skip).Take(size).ToList();
                var totalCount = followers.Count();

                return new FollowerPagedModel
                {
                    Records = resultSet,
                    TotalCount = totalCount
                };
            }
        }

        public IEnumerable<string> GetFollowingUsers(string userId)
        {
            using (var entities = new SocialFeedEntities())
            {
                return entities.UserFollower.Where(p => p.FollowerId == userId).Select(p => p.UserId).ToList();
            }
        }

        public FollowerPagedModel GetFollowingUsersPaged(string userId, int skip, int size)
        {
            using (var entities = new SocialFeedEntities())
            {
                var users = entities.UserFollower.Where(p => p.FollowerId == userId);
                var resultSet = users.OrderBy(p => p.UserId).Skip(skip).Take(size).Select(p=>p.UserId).ToList();
                var totalCount = users.Count();

                return new FollowerPagedModel
                {
                    Records = resultSet,
                    TotalCount = totalCount
                };
            }
        }

        public IEnumerable<WallModel> GetFollowingGroups(string userId)
        {
            //TODO:Update Private/Public condition
            using (var entities = new SocialFeedEntities())
            {
                return entities.GroupFollower.Where(p => p.FollowerId == userId).Select(p => new WallModel
                {
                    OwnerId = p.GroupId,
                    IsPublic = true,
                    WallOwnerType = WallType.group
                }).ToList();
            }
        }

        public FollowerPagedModel GetFollowingGroupsPaged(string userId, int skip, int size)
        {
            using (var entities = new SocialFeedEntities())
            {
                var groups = entities.GroupFollower.Where(p => p.FollowerId == userId);
                var resultSet = groups.OrderBy(p => p.GroupId).Skip(skip).Take(size).Select(p => p.GroupId).ToList();

                var totalCount = groups.Count();

                return new FollowerPagedModel
                {
                    Records = resultSet,
                    TotalCount = totalCount
                };
            }
        }

    }
}
