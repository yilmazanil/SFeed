using SFeed.Core.Models.Newsfeed;
using System;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IWallPostCacheManager : IDisposable
    {
        void AddPost(WallPostCacheModel wallPost);
        void UpdatePost(WallPostCacheModel wallPost);
        void DeletePost(string Id);
    }
}
