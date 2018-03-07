﻿using SFeed.Core.Infrastructue.Repository;
using SFeed.Core.Infrastructure.Providers;
using SFeed.RedisRepository;
using SFeed.SqlRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using SFeed.Core.Models;

namespace SFeed.Business.Providers
{
    public class FollowerProvider : IFollowerProvider
    {
        private readonly IRepository<UserFollower> userFollowerRepo;
        private readonly IRepository<GroupFollower> groupFollowerRepo;
        private readonly INamedCacheListRepository<string> userFollowerCacheRepo;
        private readonly INamedCacheListRepository<string> groupFollowerCacheRepo;

        public FollowerProvider() : this(
            new UserFollowerRepository(),
            new GroupFollowerRepository(),
            new RedisUserFollowerRepository(),
            new RedisGroupFollowerRepository()
            )
        {

        }

        public FollowerProvider(
            IRepository<UserFollower> userFollowerRepo,
            IRepository<GroupFollower> groupFollowerRepo,
            INamedCacheListRepository<string> userFollowerCacheRepo,
            INamedCacheListRepository<string> groupFollowerCacheRepo)
        {
            this.userFollowerRepo = userFollowerRepo;
            this.groupFollowerRepo = groupFollowerRepo;
            this.userFollowerCacheRepo = userFollowerCacheRepo;
            this.groupFollowerCacheRepo = groupFollowerCacheRepo;
        }
        public void FollowUser(string followerId, string userId)
        {
            bool alreadyFollowing = userFollowerRepo.Any(p => p.FollowerId == followerId && p.UserId == userId);
            if (!alreadyFollowing)
            {
                userFollowerRepo.Add(new UserFollower { FollowerId = followerId, UserId = userId });
                userFollowerRepo.CommitChanges();
                userFollowerCacheRepo.AppendToList(userId, followerId);
            }
        }

        public void UnfollowUser(string followerId, string userId)
        {
            userFollowerRepo.Delete(f => f.FollowerId == followerId && f.UserId == userId);
            userFollowerRepo.CommitChanges();
            userFollowerCacheRepo.RemoveFromList(userId, followerId);
        }

        public void FollowGroup(string followerId, string groupId)
        {
            bool alreadyFollowing = groupFollowerRepo.Any(p => p.FollowerId == followerId && p.GroupId == groupId);
            if (!alreadyFollowing)
            {
                groupFollowerRepo.Add(new GroupFollower { FollowerId = followerId, GroupId = groupId });
                groupFollowerRepo.CommitChanges();
                groupFollowerCacheRepo.AppendToList(groupId, followerId);
            }
        }

        public void UnfollowGroup(string followerId, string groupId)
        {
            groupFollowerRepo.Delete(f => f.FollowerId == followerId && f.GroupId == groupId);
            groupFollowerRepo.CommitChanges();
            groupFollowerCacheRepo.RemoveFromList(groupId, followerId);
        }

        public IEnumerable<string> GetFollowers(IEnumerable<Actor> actors)
        {
            var userIdList = new List<string>();

            foreach (var actor in actors)
            {
                var followers = GetFollowers(actor);
                if (followers != null)
                {
                    userIdList.AddRange(followers);
                }

            }
            return userIdList.Distinct();
        }

        public IEnumerable<string> GetFollowers(Actor actor)
        {
            IEnumerable<string> followers;
            if (actor.ActorTypeId == (short)ActorType.user)
            {
                followers = userFollowerCacheRepo.GetList(actor.Id);

                if (!followers.Any())
                {
                    followers = userFollowerRepo.GetMany(p => p.UserId == actor.Id).Select(u => u.FollowerId);
                    userFollowerCacheRepo.RecreateList(actor.Id, followers);
                    return followers;
                }
            }
            else
            {
                followers = groupFollowerCacheRepo.GetList(actor.Id);

                if (!followers.Any())
                {
                    followers = groupFollowerRepo.GetMany(p => p.GroupId == actor.Id).Select(u => u.FollowerId);
                    groupFollowerCacheRepo.RecreateList(actor.Id, followers);
                    return followers;
                }
            }

            return followers != null ? followers.Distinct() : null; 
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (userFollowerRepo != null)
                {
                    userFollowerRepo.Dispose();
                }
                if (groupFollowerRepo != null)
                {
                    groupFollowerRepo.Dispose();
                }
                if (userFollowerCacheRepo != null)
                {
                    userFollowerCacheRepo.Dispose();
                }
                if (groupFollowerCacheRepo != null)
                {
                    groupFollowerCacheRepo.Dispose();
                }
            }
        }
    }
}