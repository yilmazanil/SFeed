using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using SFeed.Core.Infrastructue.Repository;
using SFeed.RedisRepository.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFeed.RedisRepository
{
    public abstract class RedisItemRepositoryBase<T> : ICacheItemRepository<T>
    {
        private IRedisClient client;
        private IRedisTypedClient<T> clientApi;

        public abstract string ItemPrefix { get; }


        protected IRedisClient Client
        {
            get
            {
                if (client == null)
                {
                    client = RedisConnectionProvider.GetClient();
                }
                return client;
            }
        }

        protected IRedisTypedClient<T> ClientApi
        {
            get
            {
                if (clientApi == null)
                {
                    clientApi = Client.As<T>();
                }
                return clientApi;
            }
        }

        protected virtual string GetEntryKey(string itemKey)
        {
            return string.Concat(ItemPrefix, ":", itemKey);
        }

        public void AddItem(string key, T cacheItem)
        {
            var cacheKey = GetEntryKey(key);
            ClientApi.SetValue(cacheKey, cacheItem);
        }

        public void AddOrUpdateItem(string key, T cacheItem)
        {
            var cacheKey = GetEntryKey(key);
            ClientApi.SetValue(cacheKey, cacheItem);
        }

        public void RemoveItem(string key)
        {
            var cacheKey = GetEntryKey(key);
            ClientApi.RemoveEntry(cacheKey);
        }

        public IEnumerable<T> GetAllByKeys(IEnumerable<string> keys)
        {
            var cacheKeys = keys.Select(t => GetEntryKey(t));
            return Client.GetAll<T>(cacheKeys).Values;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (client != null)
                {
                    client.Dispose();
                }

            }
        }
    }
}
