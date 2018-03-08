using SFeed.Core.Models.Caching;

namespace SFeed.RedisRepository
{
    public class RedisGroupFollowerRepository : RedisListBehaviourBase<CacheListItemBaseModel>
    {
        public override string ListName => "groupFollower";
    }
}
