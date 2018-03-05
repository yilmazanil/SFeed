using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models;
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
        IWallPostCacheProvider wallPostCacheProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.userWallPostProvider = new UserWallPostProvider();
            this.userNewsfeedProvider = new UserNewsfeedProvider();
            this.wallPostCacheProvider = new WallPostCacheProvider();
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
            var newPost = GetSampleWallCreateRequest();
            var postId = wallPostProvider.AddPost(newPost);

            var newsFeedEntry = new NewsfeedEntry
            {
                From = new Actor { Id = newPost.PostedBy.Id, ActorTypeId = (short)ActorType.user },
                ReferencePostId = postId,
                TypeId = (short)NewsfeedEntryType.wallpost,
                To = new Actor { Id = newPost.WallOwner.Id, ActorTypeId = (short)ActorType.user }
            };

            userNewsfeedProvider.Add(newsFeedEntry);
            userNewsfeedProvider.Add(newsFeedEntry);


            var wallOwnerFeeds = userNewsfeedProvider.GetNewsfeed(testWallOwnerId);

            var feedItemFeeds = wallOwnerFeeds.Where(
                f => f.ReferencePostId == postId && f.TypeId == (short)NewsfeedEntryType.wallpost);

            var shouldExist = feedItemFeeds.Any();
            var shouldNotContainMultiple = feedItemFeeds.Count() == 1;

            Assert.IsTrue(shouldExist && shouldNotContainMultiple);

        }

        [TestMethod]
        public void Newsfeed_Should_Delete_WallPost_From_UserFeed()
        {
            var samplePost = wallPostProvider.GetUserWall(testWallOwnerId).FirstOrDefault();
            Assert.IsNotNull(samplePost);

            var newsFeedEntry = new NewsfeedEntry
            {
                From = new Actor { Id = samplePost.PostedBy.Id, ActorTypeId = (short)ActorType.user },
                ReferencePostId = samplePost.Id,
                TypeId = (short)NewsfeedEntryType.wallpost,
                To = new Actor { Id = testWallOwnerId, ActorTypeId = (short)ActorType.user }
            };

            userNewsfeedProvider.Add(newsFeedEntry);
            userNewsfeedProvider.Delete(newsFeedEntry);

            var wallOwnerFeeds = userNewsfeedProvider.GetNewsfeed(testWallOwnerId);
            var shouldNotExist = wallOwnerFeeds.Any(f => f.ReferencedPost != null &&
            f.TypeId == (short)NewsfeedEntryType.wallpost && f.ReferencePostId == samplePost.Id);

            Assert.IsTrue(!shouldNotExist);
        }


        [TestMethod]
        public void Newsfeed_Should_Update_WallPost_In_UserFeed()
        {

            var newPost = GetSampleWallCreateRequest();
            var postId = wallPostProvider.AddPost(newPost);

            var newsFeedEntry = new NewsfeedEntry
            {
                From = new Actor { Id = newPost.PostedBy.Id, ActorTypeId = (short)ActorType.user },
                ReferencePostId = postId,
                TypeId = (short)NewsfeedEntryType.wallpost,
                To = new Actor { Id = newPost.WallOwner.Id, ActorTypeId = (short)ActorType.user }
            };

            //Add and get feed
            userNewsfeedProvider.Add(newsFeedEntry);
            var wallOwnerFeeds = userNewsfeedProvider.GetNewsfeed(testWallOwnerId);
            var feedItem = wallOwnerFeeds.FirstOrDefault(f => f.ReferencePostId == postId && f.TypeId == (short)NewsfeedEntryType.wallpost);
            Assert.IsTrue(feedItem.ReferencePostId == postId);

            //Check if item is wallpost model
            var model = feedItem.ReferencedPost as WallPostCacheModel;
            Assert.IsNotNull(model);

            var wallPost = wallPostProvider.GetPost(model.Id);

            //Update and refetch again
            wallPost.Body = "UpdatedBody";
            wallPostProvider.UpdatePost(wallPost);

            wallOwnerFeeds = userNewsfeedProvider.GetNewsfeed(testWallOwnerId);
            feedItem = wallOwnerFeeds.FirstOrDefault(f => f.ReferencePostId == postId && f.TypeId == (short)NewsfeedEntryType.wallpost);
            Assert.IsTrue(string.Equals(wallPost.Body, feedItem.ReferencedPost.Body, StringComparison.OrdinalIgnoreCase));

        }

        [TestMethod]
        public void Newsfeed_Should_Like_WallPost_In_UserFeed()
        {

            var newsFeedAction = new NewsfeedEntry
            {
                TypeId = (short)NewsfeedEntryType.like,
                From = new Actor { ActorTypeId = (short)ActorType.user, Id = testUserId },
                ReferencePostId = Guid.NewGuid().ToString()
            };
            userNewsfeedProvider.Add(newsFeedAction);
            var wallOwnerFeeds = userNewsfeedProvider.GetNewsfeed(testWallOwnerId);

            var feedItem = wallOwnerFeeds.FirstOrDefault(f => f.TypeId == (short)NewsfeedEntryType.like
            && f.From.Id == testUserId);
            Assert.IsTrue(feedItem.ReferencePostId == newsFeedAction.ReferencePostId);

        }
    }
}
