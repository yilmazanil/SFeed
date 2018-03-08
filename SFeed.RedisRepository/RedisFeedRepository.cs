
using SFeed.Core.Models.Newsfeed;
using SFeed.RedisRepository.Base;

namespace SFeed.RedisRepository
{
    public class RedisUserFeedRepository : RedisListRepositoryBase<NewsfeedEntry>
    {
        public override string ListName => "userfeed";
    }
}
