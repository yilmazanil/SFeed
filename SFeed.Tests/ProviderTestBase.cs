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
        protected IUserWallPostProvider wallPostProvider;
        protected string testWallOwnerId = "UnitTestUserWallOwner1";
        protected string testUserId = "UnitTestUser1";
        protected string testGroupId = "UnitTestGroup1";

        [TestInitialize]
        public void InitializeCommon()
        {
            using (var followerProvider = new FollowerProvider())
            {
                followerProvider.FollowUser(testUserId, testWallOwnerId);
                followerProvider.FollowUser(testWallOwnerId, testUserId);
            }

            wallPostProvider = new UserWallPostProvider();

            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                RegisterEntityToViewModelMapper.Register(cfg);
            });
        }
        [TestCleanup]
        public void CleanupBase()
        {
            this.wallPostProvider.Dispose();
        }
        protected WallPostCreateRequest GetSampleWallCreateRequest()
        {
            var body = TestUtils.GenerateLoremIpsumText();

            return new WallPostCreateRequest
            {
                Body = body,
                PostedBy = testUserId,
                PostType = (short)WallPostType.text,
                WallOwner = new Actor { ActorTypeId = (short)ActorType.user, Id = testWallOwnerId },
            };

        }
    }
}
