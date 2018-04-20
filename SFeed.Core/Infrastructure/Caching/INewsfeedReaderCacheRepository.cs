using SFeed.Core.Models.Caching;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Caching
{
    public interface INewsfeedReaderCacheRepository
    {
        IEnumerable<NewsfeedResponseModel> GetUserFeed(string userId, int skip, int take);
    }
}
