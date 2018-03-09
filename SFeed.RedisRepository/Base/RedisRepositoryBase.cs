﻿using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace SFeed.RedisRepository.Base
{
    public abstract class RedisRepositoryBase
    {
        protected virtual IRedisClient GetClientInstance()
        {
            return RedisConnectionProvider.GetClient();
        }

        protected virtual string GetEntryKey(string cachePrefix, string id)
        {
            return string.Concat(CacheEntryPrefix, ":", id);
        }

        protected virtual string GetEntrySearchPattern(string cachePrefix)
        {
            return string.Concat(CacheEntryPrefix, ":");
        }

        protected IRedisTypedClient<T> GetTypedClientApi<T>(IRedisClient client)
        {
            return client.As<T>();
        }
    }
}
