using System;
using System.Collections.Generic;
using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Comments;
using SFeed.Core.Infrastructue.Repository;
using SFeed.SqlRepository;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models;
using AutoMapper;

namespace SFeed.Business.Providers
{
    public class CommentProvider : ICommentProvider
    {
        IUserNewsfeedProvider newsFeedProvider;
        IRepository<UserComment> commentRepo;

        public CommentProvider()
        {
            this.newsFeedProvider = new UserNewsfeedProvider();
            this.commentRepo = new CommentRepository();
        }
        public string AddComment(UserCommentModel entry)
        {
            var commentId = Guid.NewGuid().ToString();
            var dbEntry = new UserComment
            {
                Body = entry.Body,
                CreatedBy = entry.CreatedBy,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                Id = commentId,
                WallPostId = entry.WallPostId
            };
            commentRepo.Add(dbEntry);

            var newsFeedEntry = new NewsfeedEntry
            {
                Body = dbEntry.Body,
                From = new Actor { ActorTypeId = (short)ActorType.user, Id = dbEntry.CreatedBy },
                ReferencePostId = dbEntry.WallPostId,
                TypeId = (short)NewsfeedEntryType.comment
            };
            newsFeedProvider.AddNewsfeedItem(newsFeedEntry);
            return commentId;
        }

        public void DeleteComment(string commentId)
        {
            commentRepo.Delete(p=>p.Id == commentId);
            //newsFeedProvider.DeleteNewsfeedItem(newsFeedEntry);
        }

        public IEnumerable<UserCommentModel> GetComments(string postId)
        {
            var result =  commentRepo.GetMany(p => p.WallPostId == postId);
            return Mapper.Map<IEnumerable<UserCommentModel>>(result);
        }

        public void UpdateComment(UserCommentModel entry)
        {
            var dbEntry = new UserComment()
            {
                Id = entry.Id,
                Body = entry.Body,
                ModifiedDate = DateTime.Now,
                IsDeleted = false
            };

            commentRepo.Update(dbEntry);
        }
    }
}
