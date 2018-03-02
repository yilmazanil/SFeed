using SFeed.Core.Infrastructue.Services;
using System.Collections.Generic;
using SFeed.Core.Models;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;

namespace SFeed.Business.Services
{
    public class UserWallEntryService : IUserWallEntryService
    {
        IUserWallEntryProvider wallEntryProvider;
        IUserNewsfeedProvider newsFeedProvider;
        IUserFollowerProvider followerProvider;

        public UserWallEntryService(): this(
            new UserWallEntryProvider(),
            new UserNewsfeedProvider(),
            new UserFollowerProvider())
        {

        }
        public UserWallEntryService(
            IUserWallEntryProvider wallEntryProvider,
            IUserNewsfeedProvider newsFeedProvider,
            IUserFollowerProvider followerProvider)
        {
            this.wallEntryProvider = wallEntryProvider;
            this.newsFeedProvider = newsFeedProvider;
            this.followerProvider = followerProvider;
        }

        public string CreatePost(WallEntryModel model, string wallOwnerId)
        {
            var entryId =  wallEntryProvider.AddEntry(model, wallOwnerId);
            var users = new List<string> { wallOwnerId, model.CreatedBy };
            var followers = followerProvider.GetFollowers(users);
            var feedItemModel = new FeedItemModel { EntryType = (short)FeedEntryTypeEnum.WallEntry, ReferenceId = entryId };
            newsFeedProvider.AddToUserFeeds(feedItemModel, followers);
            return entryId;
        }

        public void DeletePost(string postId)
        {
            wallEntryProvider.DeleteEntry(postId);
        }

        public WallEntryModel GetPost(string postId)
        {
            return wallEntryProvider.GetEntry(postId);
        }

        public IEnumerable<WallEntryModel> GetUserWall(string wallOwnerId)
        {
            return wallEntryProvider.GetUserWall(wallOwnerId);
        }

        public void UpdatePost(WallEntryModel model)
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
