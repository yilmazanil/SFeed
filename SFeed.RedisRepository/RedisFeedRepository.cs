using SFeed.Core.Models.Newsfeed;

namespace SFeed.RedisRepository
{
    public class RedisUserFeedRepository : RedisUniqueListBase<NewsfeedEntry>
    {
        public override string ListName => "userfeed";
    }
}
