﻿using System;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Comments;
using SFeed.Core.Infrastructue.Repository;
using SFeed.SqlRepository;
using AutoMapper;
using SFeed.Core.Models.Caching;
using SFeed.RedisRepository;

namespace SFeed.Business.Providers
{
    public class UserCommentProvider : IUserCommentProvider
    {
        IRepository<UserComment> commentRepo;
        INamedCacheListRepository<CommentCacheModel> commentCacheRepo;

        public UserCommentProvider() : this(new CommentRepository(), new RedisCommentRepository())
        {

        }
        public UserCommentProvider(IRepository<UserComment> commentRepo,
              INamedCacheListRepository<CommentCacheModel> commentCacheRepo)
        {
            this.commentRepo = commentRepo;
            this.commentCacheRepo = commentCacheRepo;
        }
        public long AddComment(CommentCreateModel entry)
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
