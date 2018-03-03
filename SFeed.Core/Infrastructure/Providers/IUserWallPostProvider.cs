using SFeed.Core.Models.WallPost;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserWallPostProvider : IDisposable
    {
        string AddEntry(WallPostCreateRequest request);
        void UpdateEntry(WallPostModel model);
        void DeleteEntry(string postId);
        WallPostModel GetEntry(string postId);
        IEnumerable<WallPostModel> GetUserWall(string wallOwnerId);
        IEnumerable<WallPostModel> GetEntries(IEnumerable<string> postIds);
    }
}
