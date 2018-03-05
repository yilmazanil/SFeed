using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;
using System.Collections.Generic;
using System;

namespace SFeed.Business.Providers
{
    public class WallPostCacheManager : IWallPostCacheManager
    {
        ITypedCacheRepository<WallPostCacheModel> wallPostCacheRepo;

        public WallPostCacheManager(): this(new RedisWallPostRepository())
        {

        }
        public WallPostCacheManager(ITypedCacheRepository<WallPostCacheModel> wallPostCacheRepo)
        {
            this.wallPostCacheRepo = wallPostCacheRepo;
        }

        public void AddPost(WallPostCacheModel wallPost)
        {
            wallPostCacheRepo.AddItem(wallPost);
        }

        public void DeletePost(string Id)
        {
            wallPostCacheRepo.RemoveItem(Id);
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
                if (this.wallPostCacheRepo != null)
                {
                    wallPostCacheRepo.Dispose();
                }
            }
        }
        public WallPostCacheModel GetPost(string id)
        {
            return wallPostCacheRepo.GetItem(id);
        }

        public IEnumerable<WallPostCacheModel> GetPostsById(IEnumerable<string> ids)
        {
            return wallPostCacheRepo.GetByIds(ids);
        }

        public void UpdatePost(WallPostCacheModel wallPost)
        {
            wallPostCacheRepo.UpdateItem(wallPost.Id, wallPost);
        }
    }
}
