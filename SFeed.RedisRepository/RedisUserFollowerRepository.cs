using System;
using SFeed.Core.Models.Caching;

namespace SFeed.RedisRepository
{
    public class RedisUserFollowerRepository : RedisNamedListRepositoryBase<CacheListItemBaseModel>
    {
        public override string ListName => "userFollower";
    }
}
