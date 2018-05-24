using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using System.Linq;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.WallPost;
using System;
using log4net;

namespace SFeed.Tests.BusinessProviderTests
{
    [TestClass]
    public class UserWallPostProviderTest : ProviderTestBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(UserWallPostProviderTest));
        private IWallPostProvider wallPostProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.wallPostProvider = new WallPostProvider();
        }

        [TestMethod]
        public void Should_Create_Post()
        {
            logger.Error("Test");
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);
            var sampleGroupWall = GetRandomGroupWallOwner(true);

            var userWallPostRequest = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var groupWallPostRequest = GetSampleWallCreateRequest(sampleUser, sampleGroupWall);
            var userPostId = wallPostProvider.AddPost(userWallPostRequest);
            var groupPostId = wallPostProvider.AddPost(groupWallPostRequest);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(userPostId) && !string.IsNullOrWhiteSpace(groupPostId));
        }

        [TestMethod]
        public void Should_Create_And_Get_Post()
        {
            for (int i = 0; i < 10; i++)
            {
                var sampleUser = GetRandomUserName();
                var sampleUserWall = GetRandomUserWallOwner(true);
                var sampleGroupWall = GetRandomGroupWallOwner(true);

                var userWallPostRequest = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
                var groupWallPostRequest = GetSampleWallCreateRequest(sampleUser, sampleGroupWall);
                var userPostId = wallPostProvider.AddPost(userWallPostRequest);
                var groupPostId = wallPostProvider.AddPost(groupWallPostRequest);

                var userPostDetailedModel = wallPostProvider.GetPostDetailed(userPostId);
                var groupPostDetailedModel = wallPostProvider.GetPostDetailed(groupPostId);

                Assert.AreEqual(userPostId, userPostDetailedModel.Id);
                Assert.AreEqual(groupPostId, groupPostDetailedModel.Id);

                Assert.AreEqual(userPostDetailedModel.Body, userWallPostRequest.Body);
                Assert.AreEqual(userPostDetailedModel.PostedBy, userWallPostRequest.PostedBy);
                Assert.AreEqual(userPostDetailedModel.WallOwner.OwnerId, userWallPostRequest.TargetWall.OwnerId);

                Assert.AreEqual(groupPostDetailedModel.Body, groupWallPostRequest.Body);
                Assert.AreEqual(groupPostDetailedModel.PostedBy, groupWallPostRequest.PostedBy);
                Assert.AreEqual(groupPostDetailedModel.WallOwner.OwnerId, groupWallPostRequest.TargetWall.OwnerId);

                var userPostModel = wallPostProvider.GetPost(userPostId);
                var groupPostModel = wallPostProvider.GetPost(groupPostId);

                Assert.AreEqual(userPostModel.Id, userPostDetailedModel.Id);
                Assert.AreEqual(groupPostModel.Id, groupPostDetailedModel.Id);

                Assert.AreEqual(userPostDetailedModel.Body, userPostModel.Body);
                Assert.AreEqual(userPostDetailedModel.PostedBy, userPostModel.PostedBy);
                Assert.AreEqual(userPostDetailedModel.WallOwner.OwnerId, userPostModel.WallOwner.OwnerId);

                Assert.AreEqual(groupPostDetailedModel.Body, groupPostModel.Body);
                Assert.AreEqual(groupPostDetailedModel.PostedBy, groupPostModel.PostedBy);
                Assert.AreEqual(groupPostDetailedModel.WallOwner.OwnerId, groupPostModel.WallOwner.OwnerId);
            }

        }

        [TestMethod]
        public void Should_Create_And_Update_Post()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);

            var userWallPostRequest = GetSampleWallCreateRequest(sampleUser, sampleUserWall);

            var id = wallPostProvider.AddPost(userWallPostRequest);

            var updateRequest = new WallPostUpdateRequest
            {
                Body = "UpdatedPostBody",
                PostId = id,
                PostType = WallPostType.text
            };


            wallPostProvider.UpdatePost(updateRequest);
            var model = wallPostProvider.GetPost(id);
            Assert.AreEqual(model.Body, updateRequest.Body);
        }

        [TestMethod]
        public void Should_Delete_Post()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);
            var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var id = wallPostProvider.AddPost(request);

            wallPostProvider.DeletePost(id);

            var deletedPost = wallPostProvider.GetPost(id);
            Assert.IsNull(deletedPost);
        }
        [TestMethod]
        public void Should_Get_UserWall_Detailed()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);
            var keep = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var delete = GetSampleWallCreateRequest(sampleUser, sampleUserWall);

            var keepId = wallPostProvider.AddPost(keep);
            var deleteId = wallPostProvider.AddPost(delete);

            wallPostProvider.DeletePost(deleteId);

            var posts = wallPostProvider.GetUserWallDetailed(sampleUserWall.OwnerId, DateTime.Now, 100);

            bool shouldExist = posts.Any(p => p.Id == keepId);
            bool shouldNotExist = posts.Any(p => p.Id == deleteId);

            Assert.IsTrue(shouldExist && !shouldNotExist);

        }

        [TestMethod]
        public void Should_Get_UserWall()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);
            var keep = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var delete = GetSampleWallCreateRequest(sampleUser, sampleUserWall);

            var keepId = wallPostProvider.AddPost(keep);
            var deleteId = wallPostProvider.AddPost(delete);

            wallPostProvider.DeletePost(deleteId);

            var posts = wallPostProvider.GetUserWall(sampleUserWall.OwnerId, DateTime.Now, 100);

            bool shouldExist = posts.Any(p => p.Id == keepId);
            bool shouldNotExist = posts.Any(p => p.Id == deleteId);

            Assert.IsTrue(shouldExist && !shouldNotExist);

        }
    }
}
