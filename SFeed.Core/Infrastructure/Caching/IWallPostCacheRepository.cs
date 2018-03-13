using SFeed.Core.Models.Newsfeed;
using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface IWallPostCacheRepository
    {
        void AddItem(WallPostCacheModel model);
        void UpdateItem(WallPostUpdateRequest model, DateTime modificationDate);
        WallPostCacheModel GetItem(string postId);
        void RemoveItem(string postId);
        void RemoveItems(IEnumerable<string> postIds);
        void RemoveAll(int maxRemovalSize);
        IEnumerable<WallPostCacheModel> GetItems(IEnumerable<string> postIds);
    }
}
