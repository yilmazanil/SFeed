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
        public void Newsfeed_Should_Insert_NewWallPost_To_UserFeed_And_Check_Duplicate()
        {
            var sampleEntryId = Guid.NewGuid().ToString().ToLower();
            var request = GetSampleWallCreateRequest();
            var newsfeedModel = new NewsfeedWallPostModel
            {
                Body = request.Body,
                PostedBy = request.PostedBy,
                WallOwnerId = request.WallOwnerId,
                PostType = (short)WallPostType.plaintext,
                Id = sampleEntryId
            };


            var feedItem = new NewsfeedEntry { TypeId = (short)NewsfeedEntryType.wallpost, ReferenceEntryId = sampleEntryId };

            userNewsfeedProvider.AddToUserFeeds(newsfeedModel, NewsfeedEntryType.wallpost, new List<string> { testWallOwnerId });
            userNewsfeedProvider.AddToUserFeeds(newsfeedModel, NewsfeedEntryType.wallpost,  new List<string> { testWallOwnerId });


            var wallOwnerFeeds = userNewsfeedProvider.GetUserFeed(testWallOwnerId);

            var feedItemFeeds = wallOwnerFeeds.Where(f => f.ItemId == sampleEntryId && f.ItemType == NewsfeedEntryType.wallpost);

            var shouldExist = feedItemFeeds.Any();
            var shouldNotContainMultiple = feedItemFeeds.Count() == 1;

            Assert.IsTrue(shouldExist && shouldNotContainMultiple);

        }

        [TestMethod]
        public void Newsfeed_Should_Delete_WallPost_From_UserFeed()
        {
            var sampleEntryId = Guid.NewGuid().ToString().ToLower();
            var request = GetSampleWallCreateRequest();
            var newsfeedModel = new NewsfeedWallPostModel
            {
                Body = request.Body,
                PostedBy = request.PostedBy,
                WallOwnerId = request.WallOwnerId,
                PostType = (short)WallPostType.plaintext,
                Id = sampleEntryId
            };

            var removeItemModel = new NewsfeedEntry { TypeId = (short)NewsfeedEntryType.wallpost, ReferenceEntryId = sampleEntryId };

            userNewsfeedProvider.AddToUserFeeds(newsfeedModel, NewsfeedEntryType.wallpost, new List <string> { testWallOwnerId });
            userNewsfeedProvider.RemoveFromFeed(removeItemModel, new List <string> { testWallOwnerId });

            var wallOwnerFeeds = userNewsfeedProvider.GetUserFeed(testWallOwnerId);
            var shouldNotExist = wallOwnerFeeds.Any(f => f.ItemId == sampleEntryId && f.ItemType == NewsfeedEntryType.wallpost);

            Assert.IsTrue(!shouldNotExist);
        }


        [TestMethod]
        public void Newsfeed_Should_Update_WallPost_In_UserFeed()
        {
            var sampleEntryId = Guid.NewGuid().ToString().ToLower();
            var request = GetSampleWallCreateRequest();
            var newsfeedModel = new NewsfeedWallPostModel
            {
                Body = request.Body,
                PostedBy = request.PostedBy,
                WallOwnerId = request.WallOwnerId,
                PostType = (short)WallPostType.plaintext,
                Id = sampleEntryId
            };

            //Add and get feed
            userNewsfeedProvider.AddToUserFeeds(newsfeedModel, NewsfeedEntryType.wallpost, new List<string> { testWallOwnerId });
            var wallOwnerFeeds = userNewsfeedProvider.GetUserFeed(testWallOwnerId);
            var feedItem = wallOwnerFeeds.FirstOrDefault(f => f.ItemId == sampleEntryId && f.ItemType == NewsfeedEntryType.wallpost);
            Assert.IsTrue(feedItem.ItemId == newsfeedModel.Id);

            //Check if item is wallpost model
            var model = feedItem.Item as NewsfeedWallPostModel;
            Assert.IsNotNull(model);

            //Update and refetch again
            model.Body = "UpdatedBody";
            userNewsfeedProvider.UpdateFeed(model, NewsfeedEntryType.wallpost);

            wallOwnerFeeds = userNewsfeedProvider.GetUserFeed(testWallOwnerId);
            feedItem = wallOwnerFeeds.FirstOrDefault(f => f.ItemId == sampleEntryId && f.ItemType == NewsfeedEntryType.wallpost);
            Assert.IsTrue(string.Equals(model.Body, feedItem.Item.Body, StringComparison.OrdinalIgnoreCase));

        }
    }
}
