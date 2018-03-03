using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using AutoMapper;
using SFeed.Business.MapperConfig;
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

        [TestMethod]
        public void Should_Create_Post()
        {
            var request = GetSampleWallCreateRequest();
            var id = userWallPostProvider.AddEntry(request);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(id));
        }

        [TestMethod]
        public void Should_Create_And_Get_Post()
        {
            var request = GetSampleWallCreateRequest();
            var id = userWallPostProvider.AddEntry(request);

            var model = userWallPostProvider.GetEntry(id);
            Assert.AreEqual(model.Id, id);
        }

        [TestMethod]
        public void Should_Create_And_Validate_Post()
        {
            var request = GetSampleWallCreateRequest();
            var id = userWallPostProvider.AddEntry(request);

            //TODO:Update with comparer
            var model = userWallPostProvider.GetEntry(id);
            Assert.AreEqual(model.Id, id);
            Assert.AreEqual(model.Body, request.Body);
            Assert.AreEqual(model.PostedBy, request.PostedBy);
            Assert.AreEqual(model.PostType, (short)request.PostType);

        }
        [TestMethod]
        public void Should_Create_And_Update_Post()
        {
            var updatedBodyText = "Test_Updated";

            var request = GetSampleWallCreateRequest();
            var id = userWallPostProvider.AddEntry(request);

            var model = userWallPostProvider.GetEntry(id);
            model.Body = updatedBodyText;
            userWallPostProvider.UpdateEntry(model);

            model = userWallPostProvider.GetEntry(id);
            Assert.AreEqual(model.Body, updatedBodyText);
        }

        [TestMethod]
        public void Should_Delete_Post()
        {
            var request = GetSampleWallCreateRequest();
            var id = userWallPostProvider.AddEntry(request);

            userWallPostProvider.DeleteEntry(id);
            var deletedPost = userWallPostProvider.GetEntry(id);
            Assert.IsNull(deletedPost);
        }
        [TestMethod]
        public void Should_Get_UserWall()
        {
            var request = GetSampleWallCreateRequest();
            var request2 = GetSampleWallCreateRequest();
            var existing = userWallPostProvider.AddEntry(request);
            var deleted = userWallPostProvider.AddEntry(request2);
            userWallPostProvider.DeleteEntry(deleted);

            var posts = userWallPostProvider.GetUserWall(testWallOwnerId);

            bool shouldExist = posts.Any(p => p.Id == existing);
            bool shouldNotExist = posts.Any(p => p.Id == deleted);

            Assert.IsTrue(shouldExist && !shouldNotExist);

        }
    }
}
