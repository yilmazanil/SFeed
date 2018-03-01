using SFeed.Core.Models;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserWallService : IDisposable
    {
        Guid PublishEntryToUserWall(WallEntryModel entry, string wallOwnerUserId);
        IEnumerable<WallEntryModel> GetUserWall(string userId);
    }
}
