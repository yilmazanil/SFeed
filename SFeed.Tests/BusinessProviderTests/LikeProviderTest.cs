using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using SFeed.Core.Infrastructure.Providers;
using System.Linq;

namespace SFeed.Tests.BusinessProviderTests
{
    [TestClass]
    public class LikeProviderTest : ProviderTestBase
    {
        IUserWallPostProvider wallPostProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.wallPostProvider = new UserWallPostProvider();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.wallPostProvider.Dispose();
        }



        [TestMethod]
        public void Should_Like_And_Dislike()
        {
            var samplePostId = wallPostProvider.GetUserWall(testWallOwnerId).FirstOrDefault().Id;
            using (var likeProvider = new UserLikeProvider())
            {
                var likes =  likeProvider.GetPostLikes(samplePostId);

                var initialLikeCount = likes != null ? likes.Count() : 0;

                likeProvider.LikePost(samplePostId, testUserId);
                likeProvider.LikePost(samplePostId, testUserId);

                likes = likeProvider.GetPostLikes(samplePostId);

                var shouldExist = likes.Contains(testUserId);
                var shouldExistOnce = likes.Count(l => l == testUserId) == 1;
                var shouldIncrementByOne = initialLikeCount + 1 == likes.Count();

                Assert.IsTrue(shouldExist && shouldExistOnce && shouldIncrementByOne);

                likeProvider.UnlikePost(samplePostId, testUserId);

                likes = likeProvider.GetPostLikes(samplePostId);
                var shouldNotExist = likes.Contains(testUserId);
                var shouldDecrementByOne = initialLikeCount  == likes.Count();

                Assert.IsTrue(!shouldNotExist && shouldDecrementByOne);


            }
        }
    }
}
