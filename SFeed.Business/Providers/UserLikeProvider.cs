using SFeed.Core.Infrastructue.Repository;
using SFeed.Core.Infrastructure.Providers;
using SFeed.SqlRepository;
using System;

namespace SFeed.Business.Providers
{
    public class UserLikeProvider : IUserLikeProvider
    {
        IRepository<WallPostLike> wallPostLikeRepo;
        IRepository<UserCommentLike> userCommentLikeRepo;

        public UserLikeProvider()
        {
                
        }
        public UserLikeProvider(IRepository<WallPostLike> wallPostLikeRepo,
            IRepository<UserCommentLike> userCommentLikeRepo)
        {
            this.wallPostLikeRepo = wallPostLikeRepo;
            this.userCommentLikeRepo = userCommentLikeRepo;
        }
        public void LikeComment(long commentId, string userId)
        {
            userCommentLikeRepo.Add(new UserCommentLike { CommentId = commentId, CreatedBy = userId, CreatedDate = DateTime.Now });
            userCommentLikeRepo.CommitChanges();
        }
        public void UnlikeComment(long commentId, string userId)
        {
            userCommentLikeRepo.Delete(u => u.CreatedBy == userId && u.CommentId == commentId);
            userCommentLikeRepo.CommitChanges();
        }

        public void LikePost(string postId, string userId)
        {
            wallPostLikeRepo.Add(new WallPostLike { WallPostId = postId, CreatedBy = userId, CreatedDate = DateTime.Now });
            wallPostLikeRepo.CommitChanges();
        }

      
        public void UnlikePost(string postId, string userId)
        {
            wallPostLikeRepo.Delete(u => u.CreatedBy == userId && u.WallPostId == postId);
            wallPostLikeRepo.CommitChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (wallPostLikeRepo != null)
                {
                    wallPostLikeRepo.Dispose();
                }
                if (userCommentLikeRepo != null)
                {
                    userCommentLikeRepo.Dispose();
                }
            }
        }
    }
}
