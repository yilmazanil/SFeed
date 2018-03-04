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
        private IRedisTypedClient<T> clientApi;

        protected IRedisTypedClient<T> ClientApi {

           get {
                if (clientApi != null)
                {
                    return clientApi;
                }
                else
                {
                    InitializeClient();
                    return clientApi;
                }

            }
        }

        public RedisTypedRepositoryBase()
        {
            
        }

        protected virtual void InitializeClient()
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
            return ClientApi.Store(cacheItem);
        }

        public T GetItem(string id)
        {
            return ClientApi.GetById(id);
        }

        public void RemoveItem(string id)
        {
            ClientApi.DeleteById(id);
        }

        public IEnumerable<T> GetByIds(IEnumerable<string> ids)
        {
            return ClientApi.GetByIds(ids);
        }

        public T UpdateItem(string id, T cacheItem)
        {
            RemoveItem(id);
            return AddItem(cacheItem);
        }

    }
}
