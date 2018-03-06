using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;
using SFeed.Core.Models.Comments;
using System;
using System.Collections.Generic;

namespace SFeed.Business.Providers
{
    public class WallPostCacheProvider : IWallPostCacheManager
    {
        ITypedCacheRepository<WallPostCacheModel> wallPostCacheRepo;

        public WallPostCacheProvider(): this(new RedisWallPostRepository())
        {

        }
        public WallPostCacheProvider(ITypedCacheRepository<WallPostCacheModel> wallPostCacheRepo)
        {
            this.wallPostCacheRepo = wallPostCacheRepo;
        }

        public void AddComment(UserCommentModel comment)
        {
            var post = wallPostCacheRepo.GetItem(comment.WallPostId);
            if (post.LatestComments == null)
            {
                post.LatestComments = new List<UserCommentModel> {
                    comment
                };
            }
            else
            {
                if (post.LatestComments.Count > 3)
                {

                    post.LatestComments.RemoveAt(0);
                    post.LatestComments.Add(comment);
                }
            }
            wallPostCacheRepo.UpdateItem(comment.WallPostId, post);
        }

        public void AddPost(WallPostCacheModel wallPost)
        {
            wallPostCacheRepo.AddItem(wallPost);
        }

        public void DeleteComment(string postId, string commentId)
        {
            var post = wallPostCacheRepo.GetItem(postId);
            if (post.LatestComments != null)
            {
                var index = post.LatestComments.FindIndex(c => c.Id == commentId);
                if (index >= 0)
                {
                    post.LatestComments.RemoveAt(index);
                }
                wallPostCacheRepo.UpdateItem(postId, post);
            }
        }

        public void DeletePost(string Id)
        {
            wallPostCacheRepo.RemoveItem(Id);
        }

        public void Dispose()
        {
            if (this.wallPostCacheRepo != null)
            {
                wallPostCacheRepo.Dispose();
            }
        }

        public void UpdateComment(string commentBody, string commentId, string postId)
        {
            var post = wallPostCacheRepo.GetItem(postId);
            if (post.LatestComments != null)
            {
                var index = post.LatestComments.FindIndex(c => c.Id == commentId);
                if (index >= 0)
                {
                    post.LatestComments[index].Body = commentBody;
                    post.LatestComments[index].ModifiedDate = DateTime.Now;
                }
                wallPostCacheRepo.UpdateItem(postId, post);
            }
        }

        public void UpdatePost(WallPostCacheModel wallPost)
        {
            wallPostCacheRepo.UpdateItem(wallPost.Id, wallPost);
        }
    }
}
