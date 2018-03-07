using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Core.Infrastructue.Services;
using SFeed.Business.Services;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Business.Providers;
using SFeed.Core.Models.Newsfeed;
using System.Linq;
using SFeed.Core.Models;

namespace SFeed.Tests.BusinessServiceTests
{
    [TestClass]
    public class NewsfeedServiceTest : ProviderTestBase
    {
        IUserNewsfeedService userNewsfeedService;
        INewsfeedProvider userNewsfeedProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.userNewsfeedService = new UserNewsfeedService();
            this.userNewsfeedProvider = new UserNewsfeedProvider();
        }
        [TestCleanup]
        public void Cleanup()
        {
            this.userNewsfeedService.Dispose();
            this.userNewsfeedProvider.Dispose();
        }
        [TestMethod]
        public void Newsfeed_Should_Create_And_Get_Wall_Posts()
        {

            var newPost = GetSampleWallCreateRequest();
            var postId = wallPostProvider.AddPost(newPost);

            var newsFeedEntry = new NewsfeedEntry
            {
                From = new Actor { Id = newPost.PostedBy.Id, ActorTypeId = (short)ActorType.user },
                ReferencePostId = postId,
                TypeId = (short)NewsfeedEntryType.wallpost,
                To = new Actor { Id = newPost.WallOwner.Id, ActorTypeId = (short)ActorType.user }
            };

            userNewsfeedProvider.AddNewsfeedItem(newsFeedEntry);

            var userFeed = userNewsfeedService.GetUserNewsfeed(testWallOwnerId);

            var currentWallPost = userFeed.FirstOrDefault(p => p.TypeId == (short)NewsfeedEntryType.wallpost && p.ReferencePostId == postId);

            Assert.IsNotNull(currentWallPost);


            var canConvert = currentWallPost.ReferencedPost as WallPostCacheModel;

            Assert.IsNotNull(canConvert);


        }
    }
}
