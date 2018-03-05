using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.MapperConfig;
using SFeed.Business.Providers;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models;
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
            using (var followerProvider = new UserFollowerProvider())
            {
                followerProvider.FollowUser(testUserId, testWallOwnerId);
                followerProvider.FollowUser(testWallOwnerId, testUserId);
            }

            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                RegisterEntityToViewModelMapper.Register(cfg);
            });
        }
        [TestCleanup]
        public void Cleanup()
        {
            this.provider.Dispose();
        }
        protected WallPostCreateRequest GetSampleWallCreateRequest()
        {
            var body = TestUtils.GenerateLoremIpsumText();

            return new WallPostCreateRequest
            {
                Body = body,
                PostedBy = new Actor { ActorTypeId = (short)ActorType.user, Id = testUserId },
                PostType = (short)WallPostType.text,
                WallOwner = new Actor { ActorTypeId = (short)ActorType.user, Id = testWallOwnerId },
            };

        }
    }
}
