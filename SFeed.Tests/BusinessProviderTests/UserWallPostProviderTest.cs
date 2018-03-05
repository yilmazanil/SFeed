using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using System.Linq;
using SFeed.Core.Infrastructure.Providers;

namespace SFeed.Tests.BusinessProviderTests
{
    [TestClass]
    public class UserWallPostProviderTest : ProviderTestBase
    {
        private IUserWallPostProvider userWallPostProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.userWallPostProvider = new UserWallPostProvider();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.userWallPostProvider.Dispose();
        }

        [TestMethod]
        public void Should_Create_Post()
        {
            var request = GetSampleWallCreateRequest();
            var id = userWallPostProvider.AddPost(request);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(id));
        }

        [TestMethod]
        public void Should_Create_And_Get_Post()
        {
            var request = GetSampleWallCreateRequest();
            var id = userWallPostProvider.AddPost(request);

            var model = userWallPostProvider.GetPost(id);
            Assert.AreEqual(model.Id, id);
        }

        [TestMethod]
        public void Should_Create_And_Validate_Post()
        {
            var request = GetSampleWallCreateRequest();
            var id = userWallPostProvider.AddPost(request);

            //TODO:Update with comparer
            var model = userWallPostProvider.GetPost(id);
            Assert.AreEqual(model.Id, id);
            Assert.AreEqual(model.Body, request.Body);
            Assert.AreEqual(model.PostedBy.Id, request.PostedBy.Id);
            Assert.AreEqual(model.PostType, (short)request.PostType);

        }
        [TestMethod]
        public void Should_Create_And_Update_Post()
        {
            var updatedBodyText = "Test_Updated";

            var request = GetSampleWallCreateRequest();
            var id = userWallPostProvider.AddPost(request);

            var model = userWallPostProvider.GetPost(id);
            model.Body = updatedBodyText;
            userWallPostProvider.UpdatePost(model);

            model = userWallPostProvider.GetPost(id);
            Assert.AreEqual(model.Body, updatedBodyText);
        }

        [TestMethod]
        public void Should_Delete_Post()
        {
            var request = GetSampleWallCreateRequest();
            var id = userWallPostProvider.AddPost(request);

            userWallPostProvider.DeletePost(id);
            var deletedPost = userWallPostProvider.GetPost(id);
            Assert.IsNull(deletedPost);
        }
        [TestMethod]
        public void Should_Get_UserWall()
        {
            var request = GetSampleWallCreateRequest();
            var request2 = GetSampleWallCreateRequest();
            var existing = userWallPostProvider.AddPost(request);
            var deleted = userWallPostProvider.AddPost(request2);
            userWallPostProvider.DeletePost(deleted);

            var posts = userWallPostProvider.GetUserWall(testWallOwnerId);

            bool shouldExist = posts.Any(p => p.Id == existing);
            bool shouldNotExist = posts.Any(p => p.Id == deleted);

            Assert.IsTrue(shouldExist && !shouldNotExist);

        }
    }
}
