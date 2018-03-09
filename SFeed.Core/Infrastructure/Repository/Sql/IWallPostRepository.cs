using SFeed.Core.Models.WallPost;
using System;
using SFeed.Core.Models;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository.Sql
{
    public interface IWallPostRepository
    {
        WallPostCreateResponse SaveItem(WallPostCreateRequest model);
        DateTime UpdateItem(WallPostUpdateRequest model);
        WallPostModel GetItem(string postId);
        void RemoveItem(string postId);
        IEnumerable<WallPostModel> GetUserWall(WallOwner wallOwner, DateTime olderThan, int size);
    }
}
