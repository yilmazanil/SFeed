using SFeed.Core.Models.Newsfeed;

namespace SFeed.RedisRepository
{
    public class RedisUserFeedRepository : RedisNamedListRepositoryBase<NewsfeedEntry>
    {
        public override string ListName => "userfeed";
    }
}
