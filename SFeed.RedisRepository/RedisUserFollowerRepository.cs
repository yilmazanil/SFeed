using System;
using SFeed.Core.Models.Caching;

namespace SFeed.RedisRepository
{
    public class RedisUserFollowerRepository : RedisListBehaviourBase<CacheListItemBaseModel>
    {
        public override string ListName => "userFollower";
    }
}
