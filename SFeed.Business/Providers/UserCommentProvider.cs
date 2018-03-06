using System;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Comments;
using SFeed.Core.Infrastructue.Repository;
using SFeed.SqlRepository;
using AutoMapper;

namespace SFeed.Business.Providers
{
    public class UserCommentProvider : IUserCommentProvider
    {
        IRepository<UserComment> commentRepo;

        public UserCommentProvider() : this(new CommentRepository())
        {

        }
        public UserCommentProvider(IRepository<UserComment> commentRepo)
        {
            this.commentRepo = new CommentRepository();
        }
        public long AddComment(CommentRequestModel entry)
        {
            var dbEntry = new UserComment
            {
                Body = entry.Body,
                CreatedBy = entry.CreatedBy,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                WallPostId = entry.WallPostId
            };
            commentRepo.Add(dbEntry);
            commentRepo.CommitChanges();
            return dbEntry.Id;
        }

        public void DeleteComment(string wallPostId, long commentId)
        {
            commentRepo.Delete(p => p.Id == commentId && p.WallPostId == wallPostId);
            commentRepo.CommitChanges();
        }

        public void UpdateComment(string commentBody, long commentId, string postId)
        {
            var comment = commentRepo.Get(c => c.Id == commentId && c.WallPostId == postId);
            if (comment != null)
            {
                comment.ModifiedDate = DateTime.Now;
                comment.Body = commentBody;
                commentRepo.Update(comment);
                commentRepo.CommitChanges();
            }
        }

        public IEnumerable<CommentModel> GetComments(string postId)
        {
            var result = commentRepo.GetMany(p => p.WallPostId == postId && p.IsDeleted == false);
            return Mapper.Map<IEnumerable<CommentModel>>(result);
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
                if (commentRepo != null)
                {
                    commentRepo.Dispose();
                }
            }
        }
    }
}
