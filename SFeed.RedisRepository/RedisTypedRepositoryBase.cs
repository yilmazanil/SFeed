using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using SFeed.Core.Infrastructue.Repository;
using System;
using System.Collections.Generic;

namespace SFeed.RedisRepository
{
    public abstract class RedisTypedRepositoryBase<T>: ITypedCacheRepository<T>, IDisposable where T :class
    {
        private IRedisClient client;

        protected IRedisTypedClient<T> clientApi;

        public RedisTypedRepositoryBase()
        {
            client = RedisConnectionProvider.GetClient();
            clientApi = client.As<T>();
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public virtual T AddItem(T cacheItem)
        {
            return clientApi.Store(cacheItem);
        }

        public T GetItem(object id)
        {
            return clientApi.GetById(id);
        }

        public void RemoveItem(object id)
        {
            var itemId = Convert.ToString(id);
            clientApi.RemoveEntry(itemId);
        }

        public IEnumerable<T> GetByIds(IEnumerable<object> ids)
        {
            return clientApi.GetByIds(ids);
        }

        public void UpdateItem(object id, T cacheItem)
        {
            RemoveItem(id);
            AddItem(cacheItem);
        }
    }
}
