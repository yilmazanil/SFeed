using SFeed.Core.Models;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Providers
{
    public interface IUserWallPostProvider : IDisposable
    {
        string AddEntry(WallEntryModel model, string wallOwnerId);
        void UpdateEntry(WallEntryModel model);
        void DeleteEntry(string postId);
        WallEntryModel GetEntry(string postId);
        IEnumerable<WallEntryModel> GetUserWall(string wallOwnerId);
        IEnumerable<WallEntryModel> GetEntries(IEnumerable<string> postIds);
    }
}
