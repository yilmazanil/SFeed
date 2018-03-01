using SFeed.Core.Models;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Services
{
    public interface IUserWallEntryService : IDisposable
    {
        Guid PublishEntryToUserWall(WallEntryModel entry, string wallOwnerUserId);
        IEnumerable<WallEntryModel> GetUserWall(string userId);
        void Delete(Guid postId);
    }
}
