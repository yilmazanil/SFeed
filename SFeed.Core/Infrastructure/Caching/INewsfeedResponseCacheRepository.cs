using SFeed.Core.Models.Caching;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Caching
{
    public interface INewsfeedResponseCacheRepository
    {
        IEnumerable<NewsfeedWallPostModel> GetUserFeed(string userId, int skip, int take);
    }
}
