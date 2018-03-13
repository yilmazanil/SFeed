using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface IWallPostRepository
    {
        WallPostCreateResponse SaveItem(WallPostCreateRequest model);
        DateTime? UpdateItem(WallPostUpdateRequest model);
        WallPostModel GetItem(string postId);
        bool RemoveItem(string postId);
        IEnumerable<WallPostModel> GetUserWall(string userId, DateTime olderThan, int size);
        IEnumerable<WallPostModel> GetGroupWall(string groupId, DateTime olderThan, int size);
        IEnumerable<string> GetPostIdsByUserWall(string userId);
    }
}
