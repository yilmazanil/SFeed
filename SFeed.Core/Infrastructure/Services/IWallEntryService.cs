using SFeed.Core.Models;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Services
{
    public interface IWallEntryService : IDisposable
    {
        IEnumerable<WallEntryModel> GetEntries(IEnumerable<string> ids);
        string CreateEntry(WallEntryModel model);
        void Delete(string postId);
    }
}
