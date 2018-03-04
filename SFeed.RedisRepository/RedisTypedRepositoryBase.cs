using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using SFeed.Core.Infrastructue.Repository;
using SFeed.Core.Models.Caching;
using System;
using System.Collections.Generic;

namespace SFeed.RedisRepository
{
    public abstract class RedisTypedRepositoryBase<T>: ITypedCacheRepository<T>, IDisposable where T : TypedCacheItemBaseModel
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

        public T GetItem(string id)
        {
            return clientApi.GetById(id);
        }

        public void RemoveItem(string id)
        {
            clientApi.DeleteById(id);
        }

        public IEnumerable<T> GetByIds(IEnumerable<object> ids)
        {
            return clientApi.GetByIds(ids);
        }

        public T UpdateItem(string id, T cacheItem)
        {
            RemoveItem(id);
            return AddItem(cacheItem);
        }

        public IEnumerable<T> GetByIds(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

    }
}
