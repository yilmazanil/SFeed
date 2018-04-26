using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;
using System.Linq;
using SFeed.Core.Models;
using System.Collections.Generic;

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

        [TestMethod]
        public void Should_Return_Equal()
        {
            var targetUser = GetRandomUserName();
            var targetGroup = GetRandomGroupName();

            foreach (var user in RandomUserNames)
            {
                followerProvider.FollowUser(user, targetUser);
                followerProvider.FollowGroup(user, targetGroup);
            }

            var groupFollowersCount = followerProvider.GetGroupFollowers(targetGroup).Count();
            var userFollowersCount = followerProvider.GetUserFollowers(targetUser).Count();
            var cachedGroupFollowersCount = followerProvider.GetGroupFollowersCached(targetGroup).Count();
            var cachedUserFollowersCount = followerProvider.GetUserFollowersCached(targetUser).Count();
            var userFollowerPagedCount = followerProvider.GetUserFollowers(targetUser).Count();
            var groupFollowerPagedCount = followerProvider.GetGroupFollowers(targetGroup).Count();

            var shouldGroupFollowerCountBeEqual = groupFollowersCount == cachedGroupFollowersCount;
            var shouldUserFollowerCountBeEqual = userFollowersCount == cachedUserFollowersCount;
            

            Assert.IsTrue(shouldGroupFollowerCountBeEqual && shouldUserFollowerCountBeEqual);

            shouldGroupFollowerCountBeEqual = groupFollowersCount == groupFollowerPagedCount;
            shouldGroupFollowerCountBeEqual = userFollowersCount == userFollowerPagedCount;

            Assert.IsTrue(shouldGroupFollowerCountBeEqual && shouldUserFollowerCountBeEqual);

        }

        [TestMethod]
        public void Should_Return_Paged()
        {
            int pagingSize = 20;
            int skip = 0;
            List<string> fetchedRecords = new List<string>();

            var targetUser = GetRandomUserName();
            var targetGroup = GetRandomGroupName();

            foreach (var user in RandomUserNames)
            {
                followerProvider.FollowUser(user, targetUser);
                followerProvider.FollowGroup(user, targetGroup);
            }

            var groupFollowers = followerProvider.GetGroupFollowers(targetGroup);
            var userFollowers = followerProvider.GetUserFollowers(targetUser);

            while (true)
            {
                var result =  followerProvider.GetGroupFollowers(targetGroup);
                fetchedRecords.AddRange(result);
                //skip += pagingSize;
                //if (skip > result.TotalCount)
                //{
                    break;
                //}
            }
    
            foreach (var item in groupFollowers)
            {
                Assert.IsTrue(fetchedRecords.Contains(item));
            }

            fetchedRecords.Clear();
            skip = 0;

            while (true)
            {
                var result = followerProvider.GetUserFollowers(targetUser);
                fetchedRecords.AddRange(result);
                //skip += pagingSize;
                //if (skip > result.TotalCount)
                //{
                    break;
                //}
            }

            foreach (var item in userFollowers)
            {
                Assert.IsTrue(fetchedRecords.Contains(item));
            }

        }
    }
}
