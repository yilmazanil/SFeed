using SFeed.Core.Infrastructue.Services;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;
using SFeed.Core.Models.WallPost;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models.Caching;

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
            var entryId =  wallPostProvider.AddEntry(request);
            var users = new List<string> { request.WallOwnerId, request.PostedBy };
            var followers = followerProvider.GetFollowers(users);
            var feedItemModel = new NewsfeedEntry { TypeId = (short)NewsfeedEntryType.wallpost, ReferenceEntryId = entryId };

            var feedItem = new WallPostNewsfeedModel
            {
                Body = request.Body,
                PostedBy = request.PostedBy,
                PostType = (short)request.PostType,
                WallOwnerId = request.WallOwnerId,
                Id = entryId
            };
            newsFeedProvider.AddToUserFeeds(feedItem,  NewsfeedEntryType.wallpost, followers);
            return entryId;
        }

        public void DeletePost(string postId)
        {
            wallPostProvider.DeleteEntry(postId);
        }

        public WallPostModel GetPost(string postId)
        {
            return wallPostProvider.GetEntry(postId);
        }

        public IEnumerable<WallPostModel> GetUserWall(string wallOwnerId)
        {
            return wallPostProvider.GetUserWall(wallOwnerId);
        }

        public void UpdatePost(WallPostModel model)
        {
            wallPostProvider.UpdateEntry(model);
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
