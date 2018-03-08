using SFeed.Core.Models.Newsfeed;

namespace SFeed.RedisRepository
{
    public class RedisWallPostRepository : RedisTypedRepositoryBase<WallPostCacheModel>
    {
        public override string ListName => "wallPost";
    }
}
