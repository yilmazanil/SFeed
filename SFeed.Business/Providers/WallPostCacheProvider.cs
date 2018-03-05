using SFeed.Core.Infrastructure.Providers;
using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository;

namespace SFeed.Business.Providers
{
    public class WallPostCacheProvider : IWallPostCacheProvider
    {
        ITypedCacheRepository<WallPostCacheModel> wallPostCacheRepo;

        public WallPostCacheProvider(): this(new RedisWallPostRepository())
        {

        }
        public WallPostCacheProvider(ITypedCacheRepository<WallPostCacheModel> wallPostCacheRepo)
        {
            this.wallPostCacheRepo = wallPostCacheRepo;
        }

        public void Add(WallPostCacheModel wallPost)
        {
            wallPostCacheRepo.AddItem(wallPost);
        }

        public void Delete(string Id)
        {
            wallPostCacheRepo.RemoveItem(Id);
        }

        public void Update(WallPostCacheModel wallPost)
        {
            wallPostCacheRepo.UpdateItem(wallPost.Id, wallPost);
        }
    }
}
