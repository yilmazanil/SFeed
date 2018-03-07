using SFeed.Core.Infrastructue.Services;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;
using SFeed.Core.Models.WallPost;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models;

namespace SFeed.Business.Services
{
    public sealed class UserWallEntryService : IUserWallPostService
    {
        IUserWallPostProvider wallPostProvider;
        INewsfeedProvider newsFeedProvider;
        IFollowerProvider followerProvider;

        public UserWallEntryService(): this(
            new UserWallPostProvider(),
            new UserNewsfeedProvider(),
            new FollowerProvider())
        {

        }
        public UserWallEntryService(
            IUserWallPostProvider wallPostProvider,
            INewsfeedProvider newsFeedProvider,
            IFollowerProvider followerProvider)
        {
            this.wallPostProvider = wallPostProvider;
            this.newsFeedProvider = newsFeedProvider;
            this.followerProvider = followerProvider;
        }

        public string CreatePost(WallPostCreateRequest request)
        {
            var entryId =  wallPostProvider.AddPost(request);
            newsFeedProvider.AddPost(new WallPostCacheModel
            {
                Body = request.Body,
                Id = entryId,
                PostedBy = request.PostedBy,
                PostType = request.PostType,
                WallOwner = request.WallOwner
            });
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
