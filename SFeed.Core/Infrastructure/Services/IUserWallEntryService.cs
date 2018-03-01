using SFeed.Core.Models;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserWallService : IDisposable
    {
        void AddEntryToUserWall(string entryId, string wallOwnerUserId);
        IEnumerable<WallEntryModel> GetUserWall(string userId);
    }
}
