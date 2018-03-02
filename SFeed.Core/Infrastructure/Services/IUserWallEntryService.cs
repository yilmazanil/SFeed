using SFeed.Core.Models;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserWallEntryService : IDisposable
    {
        //Update with request models
        WallEntryModel GetPost(string postId);
        IEnumerable<WallEntryModel> GetPosts(string userId);
        string CreatePost(WallEntryModel model, string wallOwnerId);
        void UpdatePost(WallEntryModel model);
        void DeletePost(string postId);
        IEnumerable<WallEntryModel> GetUserWall(string wallOwnerId);
    }
}
