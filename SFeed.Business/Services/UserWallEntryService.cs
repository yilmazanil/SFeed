﻿using SFeed.Core.Infrastructue.Services;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;
using SFeed.Core.Models.WallPost;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models.Caching;
using SFeed.Core.Models;

namespace SFeed.Business.Services
{
    public class UserWallEntryService : IUserWallPostService
    {
        IUserWallPostProvider wallPostProvider;
        IUserNewsfeedProvider newsFeedProvider;
        IUserFollowerProvider followerProvider;

        public UserWallEntryService(): this(
            new UserWallPostProvider(),
            new UserNewsfeedProvider(),
            new UserFollowerProvider())
        {

        }
        public UserWallEntryService(
            IUserWallPostProvider wallPostProvider,
            IUserNewsfeedProvider newsFeedProvider,
            IUserFollowerProvider followerProvider)
        {
            this.wallPostProvider = wallPostProvider;
            this.newsFeedProvider = newsFeedProvider;
            this.followerProvider = followerProvider;
        }

        public string CreatePost(WallPostCreateRequest request)
        {
            var entryId =  wallPostProvider.AddPost(request);
            var newsFeedEntry = new NewsfeedEntry
            {
                From = new Actor { ActorTypeId = (short)ActorType.user, Id = request.PostedBy.Id },
                To = new Actor { ActorTypeId = (short)ActorType.user, Id = request.WallOwner.Id },
                TypeId = (short)NewsfeedEntryType.wallpost,
                ReferencePostId = entryId
            };
            newsFeedProvider.Add(newsFeedEntry);
            return entryId;
        }

        public void DeletePost(string postId)
        {
            wallPostProvider.DeletePost(postId);
        }

        public WallPostModel GetPost(string postId)
        {
            return wallPostProvider.GetPost(postId);
        }

        public IEnumerable<WallPostModel> GetUserWall(string wallOwnerId)
        {
            return wallPostProvider.GetUserWall(wallOwnerId);
        }

        public void UpdatePost(WallPostModel model)
        {
            wallPostProvider.UpdatePost(model);
        }

        public void Dispose()
        {
            if (wallPostProvider != null)
            {
                wallPostProvider.Dispose();
            }
            if (newsFeedProvider != null)
            {
                newsFeedProvider.Dispose();
            }
            if (followerProvider != null)
            {
                followerProvider.Dispose();
            }
        }
    }

}
