using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFeed.Tests.BusinessProviderTests
{
    [TestClass]
    public class NewsfeedProviderTest : ProviderTestBase
    {
        IUserWallPostProvider userWallPostProvider;
        IUserNewsfeedProvider userNewsfeedProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.userWallPostProvider = new UserWallPostProvider();
            this.userNewsfeedProvider = new UserNewsfeedProvider();
        }

        [TestMethod]
        public void Should_Insert_NewWallPost_To_UserFeed_And_Check_Duplicate()
        {
            var sampleEntryId = Guid.NewGuid().ToString().ToLower();
            var request = GetSampleWallCreateRequest();
            var newsfeedModel = new NewsfeedWallPostModel
            {
                Body = request.Body,
                PostedBy = request.PostedBy,
                WallOwnerId = request.WallOwnerId,
                PostType = (short)WallPostTypeEnum.plaintext,
                Id = sampleEntryId
            };


            var feedItem = new NewsfeedEntryModel { EntryType = (short)NewsfeedEntryTypeEnum.wallpost, ReferenceEntryId = sampleEntryId };

            userNewsfeedProvider.AddToUserFeeds(newsfeedModel, new List<string> { testWallOwnerId });
            userNewsfeedProvider.AddToUserFeeds(newsfeedModel, new List<string> { testWallOwnerId });


            var wallOwnerFeeds = userNewsfeedProvider.GetUserFeed(testWallOwnerId);

            var feedItemFeeds = wallOwnerFeeds.Where(f => f.ItemId == sampleEntryId && f.ItemType == NewsfeedEntryTypeEnum.wallpost);

            var shouldExist = feedItemFeeds.Any();
            var shouldNotContainMultiple = feedItemFeeds.Count() == 1;

            Assert.IsTrue(shouldExist && shouldNotContainMultiple);

        }

        [TestMethod]
        public void Should_Delete_WallPost_From_UserFeed()
        {
            var sampleEntryId = Guid.NewGuid().ToString().ToLower();
            var request = GetSampleWallCreateRequest();
            var newsfeedModel = new NewsfeedWallPostModel
            {
                Body = request.Body,
                PostedBy = request.PostedBy,
                WallOwnerId = request.WallOwnerId,
                PostType = (short)WallPostTypeEnum.plaintext,
                Id = sampleEntryId
            };

            var removeItemModel = new NewsfeedEntryModel { EntryType = (short)NewsfeedEntryTypeEnum.wallpost, ReferenceEntryId = sampleEntryId };

            userNewsfeedProvider.AddToUserFeeds(newsfeedModel, new List<string> { testWallOwnerId });
            userNewsfeedProvider.RemoveFromFeed(removeItemModel, new List<string> { testWallOwnerId });

            var wallOwnerFeeds = userNewsfeedProvider.GetUserFeed(testWallOwnerId);
            var shouldNotExist = wallOwnerFeeds.Any(f => f.ItemId == sampleEntryId && f.ItemType == NewsfeedEntryTypeEnum.wallpost);

            Assert.IsTrue(!shouldNotExist);
        }
    }
}
