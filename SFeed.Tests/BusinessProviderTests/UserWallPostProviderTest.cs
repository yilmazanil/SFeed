using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using System.Linq;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.WallPost;
using System;

namespace SFeed.Tests.BusinessProviderTests
{
    [TestClass]
    public class UserWallPostProviderTest : ProviderTestBase
    {
        private IWallPostProvider userWallPostProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.userWallPostProvider = new UserWallPostProvider();
        }

        [TestMethod]
        public void Should_Create_Post()
        {
            for (int i = 0; i < 1000; i++)
            {
                var sampleUser = GetRandomUserName();
                var sampleUserWall = GetRandomUserWallOwner(true);

                var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
                var id = userWallPostProvider.AddPost(request);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(id));
            }
        }

        [TestMethod]
        public void Should_Create_More_Posts()
        {
            var sampleUserWall = GetRandomUserWallOwner(true);

            foreach (var user in RandomUserNames)
            {
                var request = GetSampleWallCreateRequest(user, sampleUserWall);
                var id = userWallPostProvider.AddPost(request);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(id));
            }
            
        }

        [TestMethod]
        public void Should_Create_And_Get_Post()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);

            var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);

            var id = userWallPostProvider.AddPost(request);
            var model = userWallPostProvider.GetPost(id);
            Assert.AreEqual(model.Id, id);
            Assert.AreEqual(model.Body, request.Body);
            Assert.AreEqual(model.PostedBy, request.PostedBy);
            Assert.AreEqual(model.PostType, (short)request.PostType);
        }

        [TestMethod]
        public void Should_Create_And_Update_Post()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);
            var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var id = userWallPostProvider.AddPost(request);

            var updateRequest = new WallPostUpdateRequest
            {
                Body = "UpdatedPostBody",
                PostId = id,
                PostType = WallPostType.text
            };

            userWallPostProvider.UpdatePost(updateRequest);
            var model = userWallPostProvider.GetPost(id);

            Assert.AreEqual(model.Body, updateRequest.Body);
        }

        [TestMethod]
        public void Should_Delete_Post()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);
            var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var id = userWallPostProvider.AddPost(request);

            userWallPostProvider.DeletePost(id);

            var deletedPost = userWallPostProvider.GetPost(id);
            Assert.IsNull(deletedPost);
        }
        [TestMethod]
        public void Should_Get_UserWall()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);
            var keep = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var delete = GetSampleWallCreateRequest(sampleUser, sampleUserWall);

            var keepId = userWallPostProvider.AddPost(keep);
            var deleteId = userWallPostProvider.AddPost(delete);

            userWallPostProvider.DeletePost(deleteId);

            var posts = userWallPostProvider.GetUserWall(sampleUserWall.OwnerId , DateTime.Now, 100);

            bool shouldExist = posts.Any(p => p.Id == keepId);
            bool shouldNotExist = posts.Any(p => p.Id == deleteId);

            Assert.IsTrue(shouldExist && !shouldNotExist);

        }
    }
}
