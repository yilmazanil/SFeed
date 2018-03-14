using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserWallPostService : IDisposable
    {
        WallPostDetailedModel GetPost(string postId);
        string CreatePost(WallPostCreateRequest request);
        void UpdatePost(WallPostDetailedModel request);
        void DeletePost(string postId);
        IEnumerable<WallPostDetailedModel> GetUserWall(string wallOwnerId);
    }
}
