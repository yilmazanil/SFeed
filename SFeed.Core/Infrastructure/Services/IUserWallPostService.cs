using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserWallPostService : IDisposable
    {
        WallPostWithDetailsModel GetPost(string postId);
        string CreatePost(WallPostCreateRequest request);
        void UpdatePost(WallPostWithDetailsModel request);
        void DeletePost(string postId);
        IEnumerable<WallPostWithDetailsModel> GetUserWall(string wallOwnerId);
    }
}
