using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFeed.Tests.BusinessProviderTests
{
    [TestClass]
    public class NewsfeedProviderTest : ProviderTestBase
    {
        IUserWallPostProvider userWallPostProvider;
        IUserNewsfeedProvider userNewsfeedProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.userWallPostProvider = new UserWallPostProvider();
            this.userNewsfeedProvider = new UserNewsfeedProvider();
        }

        [TestMethod]
        public void Should_Insert_NewWallPost_To_UserFeed_And_Check_Duplicate()
        {
            var sampleEntryId = Guid.NewGuid().ToString().ToLower();

            var feedItem = new NewsfeedEntryModel { EntryType = (short)NewsfeedEntryTypeEnum.wallpost, ReferenceEntryId = sampleEntryId };

            userNewsfeedProvider.AddToUserFeeds(feedItem, new List<string> { testWallOwnerId });
            userNewsfeedProvider.AddToUserFeeds(feedItem, new List<string> { testWallOwnerId });


            var wallOwnerFeeds = userNewsfeedProvider.GetUserFeed(testWallOwnerId);

            var feedItemFeeds = wallOwnerFeeds.Where(f => f.ReferenceEntryId == sampleEntryId && f.EntryType == (short)NewsfeedEntryTypeEnum.wallpost);

            var shouldExist = feedItemFeeds.Any();
            var shouldNotContainMultiple = feedItemFeeds.Count() == 1;

            Assert.IsTrue(shouldExist && shouldNotContainMultiple);

        }

        [TestMethod]
        public void Should_Delete_WallPost_From_UserFeed()
        {
            var sampleEntryId = Guid.NewGuid().ToString().ToLower();

            var feedItem = new NewsfeedEntryModel { EntryType = (short)NewsfeedEntryTypeEnum.wallpost, ReferenceEntryId = sampleEntryId };

            userNewsfeedProvider.AddToUserFeeds(feedItem, new List<string> { testWallOwnerId });
            userNewsfeedProvider.RemoveFromFeed(feedItem, new List<string> { testWallOwnerId });

            var wallOwnerFeeds = userNewsfeedProvider.GetUserFeed(testWallOwnerId);
            var shouldNotExist = wallOwnerFeeds.Any(f => f.ReferenceEntryId == sampleEntryId && f.EntryType == (short)NewsfeedEntryTypeEnum.wallpost);

            Assert.IsTrue(!shouldNotExist);
        }
    }
}
