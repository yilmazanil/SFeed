using SFeed.Core.Infrastructue.Repository;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Infrastructure.Repository;
using SFeed.RedisRepository;
using SFeed.SqlRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFeed.Business.Providers
{
    public class UserLikeProvider : IUserLikeProvider
    {
        ICacheItemCounter likeCounter; 
        IRepository<WallPostLike> wallPostLikeRepo;
        IRepository<UserCommentLike> userCommentLikeRepo;

        public UserLikeProvider() : this(
            new WallPostLikeRepository(),
            new UserCommentLikeRepository(),
            new RedisPostLikeCounter())
        {

        }
        public UserLikeProvider(IRepository<WallPostLike> wallPostLikeRepo,
            IRepository<UserCommentLike> userCommentLikeRepo,
            ICacheItemCounter likeCounter)
        {
            this.wallPostLikeRepo = wallPostLikeRepo;
            this.userCommentLikeRepo = userCommentLikeRepo;
            this.likeCounter = likeCounter;
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
            likeCounter.Increment(postId);
        }


        public void UnlikePost(string postId, string userId)
        {
            wallPostLikeRepo.Delete(u => u.CreatedBy == userId && u.WallPostId == postId);
            wallPostLikeRepo.CommitChanges();
            likeCounter.Decrement(postId);
        }
        public IEnumerable<string> GetPostLikes(string postId)
        {
            var likes = wallPostLikeRepo.GetMany(w => w.WallPostId == postId);
            if (likes != null)
            {
                return likes.Select(w => w.CreatedBy);
            }
            return null;
        }

        public IEnumerable<string> GetCommentLikes(long commentId)
        {
            return userCommentLikeRepo.GetMany(w => w.CommentId == commentId).Select(w => w.CreatedBy);
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
