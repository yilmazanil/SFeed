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
        private string existingTestPostId = "c34bbb53-3609-423a-bc3b-c6a17c2160ff";
        private string existingTestPostId2 = "b5712424-6335-489c-9ac7-ee461ed6e7c4";
        private string wallOwnerId = "UnitTestUserWallOwner1";


        [TestInitialize]
        public void Initialize()
        {
            this.provider = new UserWallPostProvider();

            Mapper.Initialize(cfg =>
            {
                RegisterEntityToViewModelMapper.Register(cfg);
            });
        }
        [TestMethod]
        public void Should_Create_Post_And_Attach_With_UserWall()
        {
            var model = new WallEntryModel { Body = "Test", CreatedBy = "UnitTestUser1", EntryType = (short)WallEntryTypeEnum.plaintext };
            provider.AddEntry(model, wallOwnerId);
        }

        [TestMethod]
        public void Should_Get_Post()
        {
          
            var model =  provider.GetEntry(existingTestPostId);
            Assert.AreEqual(model.Id, existingTestPostId);
        }

        [TestMethod]
        public void Should_Update_Entry()
        {
            var updatedBody = "Test_Updated";
            var model = provider.GetEntry(existingTestPostId);
            model.Body = updatedBody;
            provider.UpdateEntry(model);
            model = provider.GetEntry(existingTestPostId);
            Assert.AreEqual(model.Body, updatedBody);
        }

        [TestMethod]
        public void Should_Delete_Entry()
        {
            provider.DeleteEntry(existingTestPostId2);
            var deletedPost = provider.GetEntry(existingTestPostId2);
            Assert.IsNull(deletedPost);
        }
        [TestMethod]
        public void Should_Get_User_Wall()
        {
            var posts =  provider.GetUserWall(wallOwnerId);

            bool shouldExist = posts.Any(p => p.Id == existingTestPostId);
            bool shouldNotExist = posts.Any(p => p.Id == existingTestPostId2);

            Assert.IsTrue(shouldExist && !shouldNotExist);

        }
    }
}
