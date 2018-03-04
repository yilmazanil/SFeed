using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Core.Infrastructue.Services;
using SFeed.Business.Services;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;
using SFeed.Core.Models.Newsfeed;
using System.Collections.Generic;
using System.Linq;
using SFeed.Core.Models.WallPost;
using System;

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
        public void Newsfeed_Should_Create_And_Get_Wall_Posts()
        {
            var sampleEntryId = Guid.NewGuid().ToString().ToLower();
            var request = GetSampleWallCreateRequest();
            var newsfeedModel = new WallPostNewsfeedModel
            {
                Body = request.Body,
                PostedBy = request.PostedBy,
                WallOwnerId = request.WallOwnerId,
                PostType = (short)WallPostType.plaintext,
                Id = sampleEntryId
            };

            userNewsfeedProvider.AddEntry(newsfeedModel, NewsfeedEntryType.wallpost, new List<string> { testWallOwnerId });

            var userFeed = userNewsfeedService.GetUserNewsfeed(testWallOwnerId);

            var currentWallPost = userFeed.FirstOrDefault(p => p.ItemType == NewsfeedEntryType.wallpost && p.ItemId == sampleEntryId);

            Assert.IsNotNull(currentWallPost);


            var canConvert = currentWallPost.Item as WallPostNewsfeedModel;

            Assert.IsNotNull(canConvert);


        }
    }
}
