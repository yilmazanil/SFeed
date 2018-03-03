using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using SFeed.Core.Infrastructure.Providers;
using AutoMapper;
using SFeed.Business.MapperConfig;
using System.Linq;
using SFeed.Core.Models.WallPost;

namespace SFeed.Tests.RepositoryTests
{
    [TestClass]
    public class UserWallPostProviderTest
    {
        IUserWallPostProvider provider;
        private string testWallOwnerId = "UnitTestUserWallOwner1";
        private string userId = "UnitTestUser1";

        private WallPostCreateRequest GetTestRequest()
        {
            var body = TestUtils.GenerateLoremIpsumText();

            return new WallPostCreateRequest
            {
                Body = body,
                PostedBy = userId,
                PostType = WallPostTypeEnum.plaintext,
                WallOwnerId = testWallOwnerId
            };

        }

        [TestInitialize]
        public void Initialize()
        {
            this.provider = new UserWallPostProvider();

            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                RegisterEntityToViewModelMapper.Register(cfg);
            });
        }

        [TestMethod]
        public void Should_Create_Post()
        {
            var request = GetTestRequest();
            var id = provider.AddEntry(request);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(id));
        }

        [TestMethod]
        public void Should_Create_And_Get_Post()
        {
            var request = GetTestRequest();
            var id = provider.AddEntry(request);

            var model = provider.GetEntry(id);
            Assert.AreEqual(model.Id, id);
        }

        [TestMethod]
        public void Should_Create_And_Validate_Post()
        {
            var request = GetTestRequest();
            var id = provider.AddEntry(request);

            //TODO:Update with comparer
            var model = provider.GetEntry(id);
            Assert.AreEqual(model.Id, id);
            Assert.AreEqual(model.Body, request.Body);
            Assert.AreEqual(model.PostedBy, request.PostedBy);
            Assert.AreEqual(model.PostType, (short)request.PostType);

        }
        [TestMethod]
        public void Should_Create_And_Update_Post()
        {
            var updatedBodyText = "Test_Updated";

            var request = GetTestRequest();
            var id = provider.AddEntry(request);

            var model = provider.GetEntry(id);
            model.Body = updatedBodyText;
            provider.UpdateEntry(model);

            model = provider.GetEntry(id);
            Assert.AreEqual(model.Body, updatedBodyText);
        }

        [TestMethod]
        public void Should_Delete_Post()
        {
            var request = GetTestRequest();
            var id = provider.AddEntry(request);

            provider.DeleteEntry(id);
            var deletedPost = provider.GetEntry(id);
            Assert.IsNull(deletedPost);
        }
        [TestMethod]
        public void Should_Get_User_Wall()
        {
            var request = GetTestRequest();
            var request2 = GetTestRequest();
            var existing = provider.AddEntry(request);
            var deleted = provider.AddEntry(request2);
            provider.DeleteEntry(deleted);

            var posts = provider.GetUserWall(testWallOwnerId);

            bool shouldExist = posts.Any(p => p.Id == existing);
            bool shouldNotExist = posts.Any(p => p.Id == deleted);

            Assert.IsTrue(shouldExist && !shouldNotExist);

        }
    }
}
