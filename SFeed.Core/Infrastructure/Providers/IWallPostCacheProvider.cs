using SFeed.Core.Models.Newsfeed;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IWallPostCacheProvider
    {
        void Add(WallPostCacheModel wallPost);
        void Update(WallPostCacheModel wallPost);
        void Delete(string Id);
    }
}
