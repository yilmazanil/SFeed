using SFeed.Core.Models.Newsfeed;

namespace SFeed.RedisRepository
{
    public class RedisUserFeedRepository : RedisListBehaviourBase<NewsfeedEntry>
    {
        public override string ListName => "userfeed";
    }
}
