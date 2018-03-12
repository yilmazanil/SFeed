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

        [TestInitialize]
        public void Initialize()
        {
            this.followerProvider = new FollowerProvider();
        }

        [TestMethod]
        public void Should_Follow_User_Once()
        {
            var follower = GetRandomUserName();
            var targetUser = GetRandomUserName();
            followerProvider.FollowUser(follower, targetUser);
            followerProvider.FollowUser(follower, targetUser);

            var followers = followerProvider.GetUserFollowers(targetUser);
            var followerRecords = followers.Where(f => f == follower);

            var shouldExist = followerRecords.Any();
            var shouldExistOnce = followerRecords.Count() == 1;

            Assert.IsTrue(shouldExist && shouldExistOnce);
        }

        [TestMethod]
        public void Should_Unfollow_User()
        {
            var follower = GetRandomUserName();
            var targetUser = GetRandomUserName();
            followerProvider.FollowUser(follower, targetUser);
            followerProvider.UnfollowUser(follower, targetUser);

            var followers = followerProvider.GetUserFollowers(targetUser);
            var followerRecords = followers.Where(f => f == follower);

            var shouldNotExist = followers.Any(f => f == follower);
            Assert.IsTrue(!shouldNotExist);
        }

        [TestMethod]
        public void Should_Follow_Group_Once()
        {
            var follower = GetRandomUserName();
            var targetGroup = GetRandomGroupName();
            followerProvider.FollowGroup(follower, targetGroup);
            followerProvider.FollowGroup(follower, targetGroup);

            var followers = followerProvider.GetGroupFollowers(targetGroup);
            var followerRecords = followers.Where(f => f == follower);

            var shouldExist = followers.Any();
            var shouldExistOnce = followerRecords.Count() == 1;
            Assert.IsTrue(shouldExist && shouldExistOnce);
        }

        [TestMethod]
        public void Should_Unfollow_Group()
        {
            var follower = GetRandomUserName();
            var targetGroup = GetRandomGroupName();
            followerProvider.FollowGroup(follower, targetGroup);
            followerProvider.UnfollowGroup(follower, targetGroup);

            var followers = followerProvider.GetGroupFollowers(targetGroup);
            var followerRecords = followers.Where(f => f == follower);

            var shouldNotExist = followers.Any(f => f == follower);

            Assert.IsFalse(shouldNotExist);
        }
    }
}
