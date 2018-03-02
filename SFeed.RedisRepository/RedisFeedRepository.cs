using SFeed.Core.Models;

namespace SFeed.RedisRepository
{
    public class RedisUserFeedRepository : RedisListRepositoryBase<FeedItemModel>
    {
        protected override string ListPrefix => "userfeed";
    }
}
