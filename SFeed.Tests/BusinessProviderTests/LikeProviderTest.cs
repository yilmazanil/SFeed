using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Comments;
using System.Linq;

namespace SFeed.Tests.BusinessProviderTests
{
    [TestClass]
    public class LikeProviderTest : ProviderTestBase
    {
        IWallPostProvider wallPostProvider;
        IEntryLikeProvider entryLikeProvider;
        ICommentProvider commentProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.wallPostProvider = new WallPostProvider();
            this.entryLikeProvider = new EntryLikeProvider();
            this.commentProvider = new CommentProvider();
        }

        [TestMethod]
        public void Should_Like_And_Dislike_Post()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);
            var sampleLikeUser = GetRandomUserName();

            var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var samplePostId = wallPostProvider.AddPost(request);

            entryLikeProvider.LikePost(samplePostId, sampleLikeUser);
            entryLikeProvider.LikePost(samplePostId, sampleLikeUser);

            var likes = entryLikeProvider.GetPostLikes(samplePostId);

            var shouldExist = likes.Contains(sampleLikeUser);
            var shouldExistOnce = likes.Count(l => l == sampleLikeUser) == 1;

            Assert.IsTrue(shouldExist && shouldExistOnce);

            var shouldBeEqual = entryLikeProvider.GetPostLikeCountCached(samplePostId) == 1;
            var shouldAlsoBeEqual = entryLikeProvider.GetPostLikesPaged(samplePostId, 0 , 100 ).TotalCount == 1;

            Assert.IsTrue(shouldBeEqual && shouldAlsoBeEqual);

            entryLikeProvider.UnlikePost(samplePostId, sampleLikeUser);

            likes = entryLikeProvider.GetPostLikes(samplePostId);
            var shouldNotExist = likes.Contains(sampleLikeUser);

            Assert.IsFalse(shouldNotExist);

            shouldBeEqual = entryLikeProvider.GetPostLikeCountCached(samplePostId) == 0;
            shouldAlsoBeEqual = entryLikeProvider.GetPostLikesPaged(samplePostId, 0, 100).TotalCount == 0;
            Assert.IsTrue(shouldBeEqual && shouldAlsoBeEqual);
        }

        [TestMethod]
        public void Should_Post_Like_Count_Be_Equal_With_Cache()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);

            var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var samplePostId = wallPostProvider.AddPost(request);

            foreach (var user in RandomUserNames)
            {
                entryLikeProvider.LikePost(samplePostId, user);
            }

            entryLikeProvider.UnlikePost(samplePostId, GetRandomUserName());

            var likeCount = entryLikeProvider.GetPostLikes(samplePostId).Count();
            var cachedLikeCount = entryLikeProvider.GetPostLikeCountCached(samplePostId);

            var shouldBeEqual = likeCount == cachedLikeCount;

            Assert.IsTrue(shouldBeEqual);
        }

        [TestMethod]
        public void Should_Like_And_Dislike_Comment()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);
            var sampleLikeUser = GetRandomUserName();

            var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var samplePostId = wallPostProvider.AddPost(request);

            var commentCreateRequest = new CommentCreateRequest
            {
                Body = "TestComment",
                CreatedBy = sampleUser,
                WallPostId = samplePostId
            };

            var commentId = commentProvider.AddComment(commentCreateRequest);

            entryLikeProvider.LikeComment(commentId, sampleLikeUser);
            entryLikeProvider.LikeComment(commentId, sampleLikeUser);

            var likes = entryLikeProvider.GetCommentLikes(commentId);

            var shouldExist = likes.Contains(sampleLikeUser);
            var shouldExistOnce = likes.Count(l => l == sampleLikeUser) == 1;

            Assert.IsTrue(shouldExist && shouldExistOnce);

            entryLikeProvider.UnlikeComment(commentId, sampleLikeUser);

            likes = entryLikeProvider.GetCommentLikes(commentId);
            var shouldNotExist = likes.Contains(sampleLikeUser);

            Assert.IsFalse(shouldNotExist);
        }


        [TestMethod]
        public void Should_Comment_Like_Count_Be_Equal_With_Cache()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);

            var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var samplePostId = wallPostProvider.AddPost(request);

            var commentCreateRequest = new CommentCreateRequest
            {
                Body = "TestComment",
                CreatedBy = sampleUser,
                WallPostId = samplePostId
            };

            var commentId = commentProvider.AddComment(commentCreateRequest);


            foreach (var user in RandomUserNames)
            {
                entryLikeProvider.LikeComment(commentId, user);
            }

            entryLikeProvider.UnlikeComment(commentId, GetRandomUserName());

            var likeCount = entryLikeProvider.GetCommentLikes(commentId).Count();
            var cachedLikeCount = entryLikeProvider.GetCommentLikeCountCached(commentId);

            var shouldBeEqual = likeCount == cachedLikeCount;

            Assert.IsTrue(shouldBeEqual);
        }
    }
}
