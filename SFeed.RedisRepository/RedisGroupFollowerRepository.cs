using SFeed.Core.Models.Caching;

namespace SFeed.RedisRepository
{
    public class RedisGroupFollowerRepository : RedisNamedListRepositoryBase<CacheListItemBaseModel>
    {
        public override string ListName => "groupFollower";
    }
}
