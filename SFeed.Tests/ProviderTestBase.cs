using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.MapperConfig;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.WallPost;

namespace SFeed.Tests
{
    public class ProviderTestBase
    {
        protected IUserWallPostProvider provider;
        protected string testWallOwnerId = "UnitTestUserWallOwner1";
        protected string testUserId = "UnitTestUser1";

        [TestInitialize]
        public void InitializeCommon()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                RegisterEntityToViewModelMapper.Register(cfg);
            });
        }

        protected WallPostCreateRequest GetSampleWallCreateRequest()
        {
            var body = TestUtils.GenerateLoremIpsumText();

            return new WallPostCreateRequest
            {
                Body = body,
                PostedBy = testUserId,
                PostType = WallPostTypeEnum.plaintext,
                WallOwnerId = testWallOwnerId
            };

        }
    }
}
