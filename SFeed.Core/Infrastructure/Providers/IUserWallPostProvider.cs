using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserWallPostProvider : IDisposable
    {
        string AddPost(WallPostCreateRequest request);
        void UpdatePost(WallPostModel model);
        void DeletePost(string postId);
        WallPostModel GetPost(string postId);
        IEnumerable<WallPostModel> GetUserWall(string wallOwnerId);
    }
}
