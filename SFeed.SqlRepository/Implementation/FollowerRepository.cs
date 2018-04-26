﻿using System.Linq;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Repository;

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

        public IEnumerable<string> GetFollowersGroup(string groupId)
        {
            using (var entities = new SocialFeedEntities())
            {
                return entities.GroupFollower.Where(t => t.GroupId == groupId).Select(t => t.FollowerId).ToList();
            }
        }

        public IEnumerable<string> GetFollowedUsers(string userId)
        {
            using (var entities = new SocialFeedEntities())
            {
                return entities.UserFollower.Where(p => p.FollowerId == userId).Select(p => p.UserId).ToList();
            }
        }

        public IEnumerable<string> GetFollowedGroups(string userId)
        {
            using (var entities = new SocialFeedEntities())
            {
                return entities.GroupFollower.Where(p => p.FollowerId == userId).Select(p => p.GroupId);
            }
        }
    }
}
