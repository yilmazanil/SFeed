using SFeed.Core.Models.Newsfeed;

namespace SFeed.RedisRepository
{
    public class RedisUserFeedRepository : RedisListRepositoryBase<NewsfeedAction>
    {
        protected override string ListPrefix => "userfeed";
    }
}
