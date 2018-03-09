using SFeed.Core.Models;
using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IWallPostProvider
    {
        string AddPost(WallPostCreateRequest request);
        void UpdatePost(WallPostUpdateRequest model);
        void DeletePost(string postId);
        WallPostModel GetPost(string postId);
        IEnumerable<WallPostModel> GetUserWall(WallOwner wallOwner, DateTime olderThan, int size);
    }
}
