using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Comments;
using System.Linq;

namespace SFeed.Tests.BusinessProviderTests
{
    [TestClass]
    public class CommentProviderTest : ProviderTestBase
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
        public void Should_Add_Comment_On_WallPost()
        {
            var samplePost = wallPostProvider.GetUserWall(testWallOwnerId).FirstOrDefault();

            using (var commentProvider = new UserCommentProvider())
            {
                var commentId =  commentProvider.AddComment(new CommentCreateModel { Body = "Test Comment", CreatedBy = testUserId, WallPostId = samplePost.Id });

                var postComments = commentProvider.GetComments(samplePost.Id);

                var shouldExist = postComments.Any(c => c.Id == commentId && c.CreatedBy == testUserId);

                Assert.IsTrue(shouldExist);

            }
                
        }

        [TestMethod]
        public void Should_Remove_Comment_From_WallPost()
        {
            var samplePost = wallPostProvider.GetUserWall(testWallOwnerId).FirstOrDefault();

            using (var commentProvider = new UserCommentProvider())
            {
                var commentId = commentProvider.AddComment(new CommentCreateModel { Body = "Test Comment", CreatedBy = testUserId, WallPostId = samplePost.Id });

                var postComments = commentProvider.GetComments(samplePost.Id);

                var shouldExist = postComments.Any(c => c.Id == commentId && c.CreatedBy == testUserId);

                Assert.IsTrue(shouldExist);


                commentProvider.DeleteComment(samplePost.Id, commentId);
                postComments = commentProvider.GetComments(samplePost.Id);
                var shouldNotExist = postComments.Any(c => c.Id == commentId && c.CreatedBy == testUserId);

                Assert.IsFalse(shouldNotExist);
            }

        }

    }
}
