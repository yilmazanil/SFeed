using SFeed.Core.Infrastructue.Services;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;
using SFeed.Core.Models.WallPost;
using SFeed.Core.Models.Newsfeed;

namespace SFeed.Business.Services
{
    public class UserWallEntryService : IUserWallPostService
    {
        IUserWallPostProvider wallEntryProvider;
        IUserNewsfeedProvider newsFeedProvider;
        IUserFollowerProvider followerProvider;

        public UserWallEntryService(): this(
            new UserWallPostProvider(),
            new UserNewsfeedProvider(),
            new UserFollowerProvider())
        {

        }
        public UserWallEntryService(
            IUserWallPostProvider wallEntryProvider,
            IUserNewsfeedProvider newsFeedProvider,
            IUserFollowerProvider followerProvider)
        {
            this.wallEntryProvider = wallEntryProvider;
            this.newsFeedProvider = newsFeedProvider;
            this.followerProvider = followerProvider;
        }

        public string CreatePost(WallPostCreateRequest request)
        {
            var entryId =  wallEntryProvider.AddEntry(request);
            var users = new List<string> { request.WallOwnerId, request.PostedBy };
            var followers = followerProvider.GetFollowers(users);
            var feedItemModel = new NewsfeedEntryModel { EntryType = (short)NewsfeedEntryTypeEnum.wallpost, ReferenceEntryId = entryId };
            newsFeedProvider.AddToUserFeeds(feedItemModel, followers);
            return entryId;
        }

        public void DeletePost(string postId)
        {
            wallEntryProvider.DeleteEntry(postId);
        }

        public WallPostModel GetPost(string postId)
        {
            return wallEntryProvider.GetEntry(postId);
        }

        public IEnumerable<WallPostModel> GetUserWall(string wallOwnerId)
        {
            return wallEntryProvider.GetUserWall(wallOwnerId);
        }

        public void UpdatePost(WallPostModel model)
        {
            wallEntryProvider.UpdateEntry(model);
        }

        public void Dispose()
        {
            if (wallEntryProvider != null)
            {
                wallEntryProvider.Dispose();
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
