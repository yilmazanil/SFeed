using SFeed.Core.Models.Newsfeed;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IWallPostCacheManager : IDisposable
    {
        void AddPost(WallPostCacheModel wallPost);
        void UpdatePost(WallPostCacheModel wallPost);
        void DeletePost(string Id);
        WallPostCacheModel GetPost(string id);
        IEnumerable<WallPostCacheModel> GetPostsById(IEnumerable<string> ids);
    }
}
