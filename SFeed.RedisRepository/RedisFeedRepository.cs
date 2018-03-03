using SFeed.Core.Models.Newsfeed;

namespace SFeed.RedisRepository
{
    public class RedisUserFeedRepository : RedisListRepositoryBase<NewsfeedEntryModel>
    {
        protected override string ListPrefix => "userfeed";
    }
}
