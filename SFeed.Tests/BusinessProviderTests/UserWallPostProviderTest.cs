using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using SFeed.Core.Models;
using SFeed.Core.Infrastructure.Providers;
using AutoMapper;
using SFeed.Business.MapperConfig;
using System.Linq;

namespace SFeed.Tests.RepositoryTests
{
    [TestClass]
    public class UserWallPostProviderTest
    {
        IUserWallPostProvider provider;
        private string wallOwnerId = "UnitTestUserWallOwner1";
        private WallEntryModel testModel = new WallEntryModel { Body = "Test", CreatedBy = "UnitTestUser1", EntryType = (short)WallEntryTypeEnum.plaintext };

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
            var id = provider.AddEntry(testModel, wallOwnerId);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(id));


        }

        [TestMethod]
        public void Should_Create_And_Get_Post()
        {
            var id = provider.AddEntry(testModel, wallOwnerId);

            var model = provider.GetEntry(id);
            Assert.AreEqual(model.Id, id);
        }

        [TestMethod]
        public void Should_Create_And_Update_Post()
        {
            var updatedBody = "Test_Updated";
            var id = provider.AddEntry(testModel, wallOwnerId);

            var model = provider.GetEntry(id);
            model.Body = updatedBody;
            provider.UpdateEntry(model);
            model = provider.GetEntry(id);
            Assert.AreEqual(model.Body, updatedBody);
        }

        [TestMethod]
        public void Should_Delete_Entry()
        {
            var id = provider.AddEntry(testModel, wallOwnerId);
            provider.DeleteEntry(id);
            var deletedPost = provider.GetEntry(id);
            Assert.IsNull(deletedPost);
        }
        [TestMethod]
        public void Should_Get_User_Wall()
        {
            var existing = provider.AddEntry(testModel, wallOwnerId);
            var deleted = provider.AddEntry(testModel, wallOwnerId);
            provider.DeleteEntry(deleted);

            var posts = provider.GetUserWall(wallOwnerId);

            bool shouldExist = posts.Any(p => p.Id == existing);
            bool shouldNotExist = posts.Any(p => p.Id == deleted);

            Assert.IsTrue(shouldExist && !shouldNotExist);

        }
    }
}
