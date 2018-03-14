using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFeed.Business.Providers;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Comments;
using System;
using System.Linq;

namespace SFeed.Tests.BusinessProviderTests
{
    [TestClass]
    public class CommentProviderTest : ProviderTestBase
    {
        IWallPostProvider wallPostProvider;
        IUserCommentProvider commentProvider;

        [TestInitialize]
        public void Initialize()
        {
            this.wallPostProvider = new WallPostProvider();
            this.commentProvider = new UserCommentProvider();
        }

        [TestMethod]
        public void Should_Add_Comment_On_WallPost()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);
            var sampleCommentUser = GetRandomUserName();

            var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var samplePostId = wallPostProvider.AddPost(request);

            var commentCreateRequest = new CommentCreateRequest
            {
                Body = "TestComment",
                CreatedBy = sampleCommentUser,
                WallPostId = samplePostId
            };

            var commentId = commentProvider.AddComment(commentCreateRequest);

            var postComments = commentProvider.GetComments(samplePostId, DateTime.Now, 100);

            var shouldExist = postComments.Any(c => c.Id == commentId && c.CreatedBy == sampleCommentUser);

            Assert.IsTrue(shouldExist);   
        }

        [TestMethod]
        public void Should_Remove_Comment_From_WallPost()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);
            var sampleCommentUser = GetRandomUserName();

            var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var samplePostId = wallPostProvider.AddPost(request);

            var commentCreateRequest = new CommentCreateRequest
            {
                Body = "TestComment",
                CreatedBy = sampleCommentUser,
                WallPostId = samplePostId
            };

            var commentId = commentProvider.AddComment(commentCreateRequest);

            var postComments = commentProvider.GetComments(samplePostId, DateTime.Now, 100);

            var shouldExist = postComments.Any(c => c.Id == commentId && c.CreatedBy == sampleCommentUser);

            Assert.IsTrue(shouldExist);

            commentProvider.DeleteComment(samplePostId, commentId);
            postComments = commentProvider.GetComments(samplePostId, DateTime.Now, 100);
            var shouldNotExist = postComments.Any(c => c.Id == commentId && c.CreatedBy == sampleCommentUser);

            Assert.IsFalse(shouldNotExist);
        }


        [TestMethod]
        public void Should_Update_Comment()
        {
            var sampleUser = GetRandomUserName();
            var sampleUserWall = GetRandomUserWallOwner(true);
            var sampleCommentUser = GetRandomUserName();

            var request = GetSampleWallCreateRequest(sampleUser, sampleUserWall);
            var samplePostId = wallPostProvider.AddPost(request);

            var commentCreateRequest = new CommentCreateRequest
            {
                Body = "TestComment",
                CreatedBy = sampleCommentUser,
                WallPostId = samplePostId
            };

            var commentId = commentProvider.AddComment(commentCreateRequest);


            var commentUpdateRequest = new CommentUpdateRequest
            {
                CommentId = commentId,
                PostId = samplePostId,
                Body = "UpdatedComment"
            };

            commentProvider.UpdateComment(commentUpdateRequest);

            var postComments = commentProvider.GetComments(samplePostId, DateTime.Now, 100);
            var spesificComment = commentProvider.GetComment(samplePostId, commentId);
            var shouldBeEqual = string.Equals(spesificComment.Body, commentUpdateRequest.Body, StringComparison.OrdinalIgnoreCase);
            var shouldExist = postComments.Any(c => c.Id == commentId && c.ModifiedDate.HasValue && c.Body == commentUpdateRequest.Body);

            Assert.IsTrue(shouldExist);
        }

    }
}
