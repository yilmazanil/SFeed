﻿using SFeed.Data.Infrastructure;

namespace SFeed.Data.RedisRepositories
{
    public class RedisUserFollowerRepository : RedisListRepositoryBase<string>
    {
        protected override string ListPrefix => "userfollower";
    }
}
