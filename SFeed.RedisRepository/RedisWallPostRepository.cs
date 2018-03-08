using SFeed.Core.Models.Newsfeed;

namespace SFeed.RedisRepository
{
    public class RedisWallPostRepository : RedisTypedRepository<WallPostCacheModel>
    {
        public override string ListName => "wallPost";
    }
}
