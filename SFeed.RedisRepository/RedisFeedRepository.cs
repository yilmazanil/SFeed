using SFeed.Core.Models.Newsfeed;

namespace SFeed.RedisRepository
{
    public class RedisUserFeedRepository : RedisListRepositoryBase<NewsfeedItemModel>
    {
        protected override string ListPrefix => "userfeed";
    }
}
