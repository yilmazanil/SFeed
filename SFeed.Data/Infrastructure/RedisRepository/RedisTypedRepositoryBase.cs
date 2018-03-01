using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using SFeed.Data.Infrastructure.RedisRepository;
using System;
using System.Collections.Generic;

namespace SFeed.Data.Infrastructure
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

        public void RemoveItem(string id)
        {
            var itemId = Convert.ToString(id);
            clientApi.RemoveEntry(itemId);
        }

        public IEnumerable<T> GetByIds(IEnumerable<object> ids)
        {
            return clientApi.GetByIds(ids);
        }
    }
}
