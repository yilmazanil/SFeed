using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFeed.Data.Infrastructure
{
    public interface IRedisUserFeedRepository : IRedisListRepository<int, long>
    {
        void AddToUserFeeds(IEnumerable<int> userIds, long postId);
        IEnumerable<SocialPost> GetUserFeeds(int userId);
    }
}
