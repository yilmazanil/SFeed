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
        IFollowerProvider followerProvider;
        //OK
        [TestInitialize]
        public void Initialize()
        {
            this.followerProvider = new FollowerProvider();
        }
        [TestCleanup]
        public void Cleanup()
        {
            this.followerProvider.Dispose();
        }
        [TestMethod]
        public void Should_Follow_User_Once()
        {
            followerProvider.FollowUser(testUserId, testWallOwnerId);
            followerProvider.FollowUser(testUserId, testWallOwnerId);

            var followers = followerProvider.GetFollowers(new Actor { Id = testWallOwnerId, ActorTypeId = (short)ActorType.user });
            var testUserEntries = followers.Where(f => f == testUserId);

            var shouldExist = testUserEntries.Any();

            var shouldExistOnce = testUserEntries.Count() == 1;
            Assert.IsTrue(shouldExist && shouldExistOnce);
        }

        [TestMethod]
        public void Should_Unfollow_User()
        {
            followerProvider.UnfollowUser(testUserId, testWallOwnerId);

            var followers = followerProvider.GetFollowers(new Actor { Id = testWallOwnerId, ActorTypeId = (short)ActorType.user });

            var shouldNotExist = followers.Any(f => f == testUserId);
            Assert.IsTrue(!shouldNotExist);
        }

        [TestMethod]
        public void Should_Follow_Group_Once()
        {
            followerProvider.FollowGroup(testUserId, testGroupId);
            followerProvider.FollowGroup(testUserId, testGroupId);

            var followers = followerProvider.GetFollowers(new Actor { Id = testGroupId, ActorTypeId = (short)ActorType.group });
            var testUserEntries = followers.Where(f => f == testUserId);

            var shouldExist = testUserEntries.Any();
            var shouldExistOnce = testUserEntries.Count() == 1;
            Assert.IsTrue(shouldExist && shouldExistOnce);
        }

        [TestMethod]
        public void Should_Unfollow_Group()
        {
            followerProvider.UnfollowGroup(testUserId, testGroupId);

            var followers = followerProvider.GetFollowers(new Actor { Id = testGroupId, ActorTypeId = (short)ActorType.group });

            var shouldNotExist = followers.Any(f => f == testGroupId);
            Assert.IsTrue(!shouldNotExist);
        }
    }
}
