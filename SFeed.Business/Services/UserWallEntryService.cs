using SFeed.Core.Infrastructue.Services;
using System.Collections.Generic;
using SFeed.Core.Models;
using SFeed.Core.Infrastructure.Providers;

namespace SFeed.Business.Services
{
    public class UserWallEntryService : IUserWallEntryService
    {
        IUserWallEntryProvider wallEntryProvider;
        IUserNewsfeedProvider newsFeedProvider;
        IUserFollowerProvider followerProvider;

        public string CreatePost(WallEntryModel model, string wallOwnerId)
        {
            var entryId =  wallEntryProvider.AddEntry(model, wallOwnerId);
            var followers = followerProvider.GetFollowers(new List<string> { wallOwnerId, model.CreatedBy });
            newsFeedProvider.AddToUserFeeds(new FeedItemModel { EntryType = (short)FeedEntryTypeEnum.WallEntry, ReferenceId = entryId }, followers);
            return entryId;
        }

        public void DeletePost(string postId)
        {
            wallEntryProvider.DeleteEntry(postId);
        }

        public void Dispose()
        {
            wallEntryProvider.Dispose();
        }

        public WallEntryModel GetPost(string postId)
        {
            return wallEntryProvider.GetEntry(postId);
        }

        public IEnumerable<WallEntryModel> GetPosts(string userId)
        {
            return wallEntryProvider.GetEntries(userId);
        }

        public IEnumerable<WallEntryModel> GetUserWall(string wallOwnerId)
        {
            return wallEntryProvider.GetUserWall(wallOwnerId);
        }

        public void UpdatePost(WallEntryModel model)
        {
            wallEntryProvider.UpdateEntry(model);
        }
    }
}
