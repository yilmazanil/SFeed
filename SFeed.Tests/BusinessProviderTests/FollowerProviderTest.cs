using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;
using System.Linq;
using SFeed.Core.Models;

namespace SFeed.Tests.BusinessProviderTests
{
    [TestClass]
    public class FollowerProviderTest : ProviderTestBase
    {
        IFollowerProvider userFollowerProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.userFollowerProvider = new FollowerProvider();
        }
        [TestCleanup]
        public void Cleanup()
        {
            this.userFollowerProvider.Dispose();
        }
        [TestMethod]
        public void Should_Follow_User_Once()
        {
            userFollowerProvider.FollowUser(testUserId, testWallOwnerId);
            userFollowerProvider.FollowUser(testUserId, testWallOwnerId);

            var followers = userFollowerProvider.GetFollowers(new Actor { Id = testWallOwnerId, ActorTypeId = (short)ActorType.user });

            var testUserEntries = followers.Where(f => f == testUserId);
            var shouldExist = testUserEntries.Any();
            var shouldExistOnce = testUserEntries.Count() == 1;

            Assert.IsTrue(shouldExist && shouldExistOnce);
        }

        [TestMethod]
        public void Should_Unfollow_User()
        {
            userFollowerProvider.UnfollowUser(testUserId, testWallOwnerId);

            var followers = userFollowerProvider.GetFollowers(new Actor { Id = testWallOwnerId, ActorTypeId = (short)ActorType.user });

            var shouldNotExist = followers.Any(f => f == testUserId);

            Assert.IsTrue(!shouldNotExist);
        }
    }
}
