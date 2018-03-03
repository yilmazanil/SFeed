using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Core.Infrastructue.Services;
using SFeed.Business.Services;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;
using SFeed.Core.Models.Newsfeed;
using System.Collections.Generic;
using System.Linq;
using SFeed.Core.Models.WallPost;

namespace SFeed.Tests.BusinessServiceTests
{
    [TestClass]
    public class NewsfeedServiceTest : ProviderTestBase
    {
        IUserNewsfeedService userNewsfeedService;
        IUserWallPostProvider userWallPostProvider;
        IUserNewsfeedProvider userNewsfeedProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.userNewsfeedService = new UserNewsfeedService();
            this.userWallPostProvider = new UserWallPostProvider();
            this.userNewsfeedProvider = new UserNewsfeedProvider();
        }

        [TestMethod]
        public void Should_Create_And_Get_Wall_Posts()
        {
            var request = GetSampleWallCreateRequest();
            var id = userWallPostProvider.AddEntry(request);

            var feedItem = new NewsfeedEntryModel { EntryType = (short)NewsfeedEntryTypeEnum.wallpost, ReferenceEntryId = id };

            userNewsfeedProvider.AddToUserFeeds(feedItem, new List<string> { testWallOwnerId });

            var userFeed = userNewsfeedService.GetUserFeed(testWallOwnerId);

            var currentWallPost = userFeed.FirstOrDefault(p => p.ItemType == NewsfeedEntryTypeEnum.wallpost && p.ItemId == id);

            Assert.IsNotNull(currentWallPost);


            var canConvert = currentWallPost.Item as WallPostModel;

            Assert.IsNotNull(canConvert);


        }
    }
}
