using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Caching
{
    public interface IWallPostCacheRepository
    {
        void SavePost(WallPostCacheModel model);
        void UpdatePost(WallPostUpdateRequest model, DateTime modificationDate);
        WallPostCacheModel GetPost(string postId);
        void RemovePost(string postId);
        void RemovePosts(IEnumerable<string> postIds);
        IEnumerable<WallPostCacheModel> GetItems(IEnumerable<string> postIds);
    }
}
