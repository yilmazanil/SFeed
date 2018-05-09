using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Caching;
using SFeed.Core.Models.Newsfeed;
using System.Collections.Generic;

namespace SFeed.Tests.BusinessProviderTests
{
    [TestClass]
    public class NewsfeedProviderTest : ProviderTestBase
    {
        IWallPostProvider userWallPostProvider;
        INewsfeedProvider userNewsfeedProvider;
        IFollowerProvider followerProvider;


        [TestInitialize]
        public void Initialize()
        {
            this.userWallPostProvider = new WallPostProvider();
            this.userNewsfeedProvider = new NewsfeedProvider();
            this.followerProvider = new FollowerProvider();
        }

        [TestMethod]
        public void Newsfeed_Should_Insert_NewWallPost_To_UserFeed_And_Check_Duplicate()
        {
            var sampleUserWall = GetRandomUserWallOwner(true);
            var sampleUser = sampleUserWall.OwnerId;
            foreach (var user in RandomUserNames)
            {
                followerProvider.FollowUser(user, sampleUser);
            }

            var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var samplePostId = userWallPostProvider.AddPost(request);

            var newsFeedEntry = new NewsfeedItem
            {
                By = request.PostedBy,
                ReferencePostId = samplePostId,
                FeedType = NewsfeedActionType.wallpost,
                WallOwner = new Core.Models.Wall.NewsfeedWallModel { IsPublic = true, OwnerId = sampleUserWall.OwnerId, WallOwnerType = sampleUserWall.WallOwnerType }
            };

            userNewsfeedProvider.AddNewsfeedItem(newsFeedEntry);
            userNewsfeedProvider.AddNewsfeedItem(newsFeedEntry);

            newsFeedEntry = new NewsfeedItem
            {
                By = request.PostedBy,
                ReferencePostId = samplePostId,
                FeedType = NewsfeedActionType.like,
                WallOwner = new Core.Models.Wall.NewsfeedWallModel { IsPublic = true, OwnerId = sampleUserWall.OwnerId, WallOwnerType = sampleUserWall.WallOwnerType }
            };
            userNewsfeedProvider.AddNewsfeedItem(newsFeedEntry);

            var feeds = new List<IEnumerable<NewsfeedResponseModel>>();

            foreach (var user in RandomUserNames)
            {
                feeds.Add(userNewsfeedProvider.GetUserNewsfeed(user, 0, 30));
            }
            userNewsfeedProvider.RemovePost(newsFeedEntry);
        }

        [TestMethod]
        public void Newsfeed_Should_Initialize_Newsfeed()
        {
            userNewsfeedProvider.GenerateNewsfeed("Adah");
        }
    }
}
