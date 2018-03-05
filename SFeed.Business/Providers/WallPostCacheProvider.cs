using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;

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
            if (this.wallPostCacheRepo != null)
            {
                wallPostCacheRepo.Dispose();
            }
        }

        public void UpdatePost(WallPostCacheModel wallPost)
        {
            wallPostCacheRepo.UpdateItem(wallPost.Id, wallPost);
        }
    }
}
