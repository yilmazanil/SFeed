using SFeed.Core.Models;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserWallEntryService : IDisposable
    {
        WallEntryModel GetPost(string postId);
        string CreatePost(WallEntryModel model, string wallOwnerId);
        void UpdatePost(WallEntryModel model);
        void DeletePost(string postId);
        IEnumerable<WallEntryModel> GetUserWall(string wallOwnerId);
    }
}
