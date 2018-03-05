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
        [TestCleanup]
        public void Cleanup()
        {
            this.userWallPostProvider.Dispose();
            this.userNewsfeedProvider.Dispose();
        }

        [TestMethod]
        public void Newsfeed_Should_Insert_NewWallPost_To_UserFeed_And_Check_Duplicate()
        {
            var sampleEntryId = Guid.NewGuid().ToString().ToLower();
            var request = GetSampleWallCreateRequest();
            var newsfeedModel = new WallPostNewsfeedModel
            {
                Body = request.Body,
                PostedBy = request.PostedBy,
                WallOwnerId = request.WallOwnerId,
                PostType = (short)WallPostType.text,
                Id = sampleEntryId
            };

            userNewsfeedProvider.AddPost(newsfeedModel);
            userNewsfeedProvider.AddPost(newsfeedModel);


            var wallOwnerFeeds = userNewsfeedProvider.GetNewsfeed(testWallOwnerId);

            var feedItemFeeds = wallOwnerFeeds.Where(
                f => f.ReferencePostId == sampleEntryId && f.ActionId == (short)NewsfeedActionType.wallpost);

            var shouldExist = feedItemFeeds.Any();
            var shouldNotContainMultiple = feedItemFeeds.Count() == 1;

            Assert.IsTrue(shouldExist && shouldNotContainMultiple);

        }

        [TestMethod]
        public void Newsfeed_Should_Delete_WallPost_From_UserFeed()
        {
            var sampleEntryId = Guid.NewGuid().ToString().ToLower();
            var request = GetSampleWallCreateRequest();
            var newsfeedModel = new WallPostNewsfeedModel
            {
                Body = request.Body,
                PostedBy = request.PostedBy,
                WallOwnerId = request.WallOwnerId,
                PostType = (short)WallPostType.text,
                Id = sampleEntryId
            };
            userNewsfeedProvider.AddPost(newsfeedModel);
            userNewsfeedProvider.DeletePost(newsfeedModel.Id);

            var wallOwnerFeeds = userNewsfeedProvider.GetNewsfeed(testWallOwnerId);
            var shouldNotExist = wallOwnerFeeds.Any(f => f.ReferencedPost != null &&
            f.ActionId == (short)NewsfeedActionType.wallpost && f.ReferencePostId == newsfeedModel.Id);

            Assert.IsTrue(!shouldNotExist);
        }


        [TestMethod]
        public void Newsfeed_Should_Update_WallPost_In_UserFeed()
        {
            var sampleEntryId = Guid.NewGuid().ToString().ToLower();
            var request = GetSampleWallCreateRequest();
            var newsfeedModel = new WallPostNewsfeedModel
            {
                Body = request.Body,
                PostedBy = request.PostedBy,
                WallOwnerId = request.WallOwnerId,
                PostType = (short)WallPostType.text,
                Id = sampleEntryId
            };

            //Add and get feed
            userNewsfeedProvider.AddPost(newsfeedModel);
            var wallOwnerFeeds = userNewsfeedProvider.GetNewsfeed(testWallOwnerId);
            var feedItem = wallOwnerFeeds.FirstOrDefault(f => f.ReferencedPost.Id == sampleEntryId && f.ActionId == (short)NewsfeedActionType.wallpost);
            Assert.IsTrue(feedItem.ReferencedPost.Id == newsfeedModel.Id);

            //Check if item is wallpost model
            var model = feedItem.ReferencedPost as WallPostNewsfeedModel;
            Assert.IsNotNull(model);

            //Update and refetch again
            model.Body = "UpdatedBody";
            userNewsfeedProvider.UpdatePost(model);

            wallOwnerFeeds = userNewsfeedProvider.GetNewsfeed(testWallOwnerId);
            feedItem = wallOwnerFeeds.FirstOrDefault(f => f.ReferencedPost.Id == sampleEntryId && f.ActionId == (short)NewsfeedActionType.wallpost);
            Assert.IsTrue(string.Equals(model.Body, feedItem.ReferencedPost.Body, StringComparison.OrdinalIgnoreCase));

        }

        [TestMethod]
        public void Newsfeed_Should_Like_WallPost_In_UserFeed()
        {

            var newsFeedAction = new NewsfeedAction
            {
                ActionId = (short)NewsfeedActionType.like,
                From = testUserId,
                ReferencePostId = Guid.NewGuid().ToString()
            };
            userNewsfeedProvider.AddAction(newsFeedAction);
            var wallOwnerFeeds = userNewsfeedProvider.GetNewsfeed(testWallOwnerId);

            var feedItem = wallOwnerFeeds.FirstOrDefault(f => f.ActionId == (short)NewsfeedActionType.like
            && f.From == testUserId);
            Assert.IsTrue(feedItem.ReferencePostId == newsFeedAction.ReferencePostId);

        }
    }
}
