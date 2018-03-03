using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserWallPostService : IDisposable
    {
        WallPostModel GetPost(string postId);
        string CreatePost(WallPostCreateRequest request);
        void UpdatePost(WallPostModel request);
        void DeletePost(string postId);
        IEnumerable<WallPostModel> GetUserWall(string wallOwnerId);
    }
}
