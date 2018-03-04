using SFeed.Core.Models.Newsfeed;

namespace SFeed.RedisRepository
{
    public class RedisUserFeedRepository : RedisListRepositoryBase<NewsfeedEntry>
    {
        protected override string ListPrefix => "userfeed";
    }
}
