using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;

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
            return string.Concat(cachePrefix, ":", id);
        }

        protected virtual string GetEntrySearchPattern(string cachePrefix)
        {
            return string.Concat(cachePrefix, ":");
        }

        protected IRedisTypedClient<T> GetTypedClientApi<T>(IRedisClient client)
        {
            return client.As<T>();
        }

        protected void Increment(string key)
        {
            using (var client = GetClientInstance())
            {
                client.Increment(key, 1);
            }
        }

        protected void Decrement(string key)
        {
            using (var client = GetClientInstance())
            {
                var value = client.GetValue(key);
                if (!string.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > 0)
                {
                    client.Decrement(key, 1);
                }
            }
        }
    }
}
