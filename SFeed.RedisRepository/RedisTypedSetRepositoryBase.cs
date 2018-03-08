using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using SFeed.Core.Infrastructue.Repository;
using System;
using System.Collections.Generic;

namespace SFeed.RedisRepository
{
    public abstract class RedisTypedSetRepositoryBase<T> : ITypedCacheSetRepository<T>
    {
        private IRedisClient client;
        private IRedisTypedClient<T> clientApi;

        public abstract string ListName { get; }

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

        protected virtual string GetEntryName(string itemId)
        {
            return string.Concat("urn:" + ListName, ":", itemId);
        }

        public bool Contains(string setId, T value)
        {
            var setName = GetEntryName(setId);
            return ClientApi.Sets[setName].Contains(value);
        }

        public void RemoveItem(string setId, T value)
        {
            var setName = GetEntryName(setId);
            var set = ClientApi.Sets[setName].Remove(value);
        }

        public void AddItem(string setId, T item)
        {
            var setName = GetEntryName(setId);
            ClientApi.Sets[setName].Add(item);
        }

        public IEnumerable<T> GetItems(string setId)
        {
            var setName = GetEntryName(setId);
            return ClientApi.Sets[setName].GetAll();
        }

        public void Clear(string setId)
        {
            var setName = GetEntryName(setId);
            ClientApi.Sets[setName].Clear();
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

        public void RecreateSet(string setId, IEnumerable<T> values)
        {
            Clear(setId);
            var setName = GetEntryName(setId);
            var set = ClientApi.Sets[setName];

            foreach (var item in values)
            {
                set.Add(item);
            }

        }
    }
}
