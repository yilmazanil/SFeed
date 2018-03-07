using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using SFeed.Core.Infrastructue.Repository;
using SFeed.Core.Models.Caching;
using System;
using System.Collections.Generic;

namespace SFeed.RedisRepository
{
    public abstract class RedisItemRepositoryBase<T> : ICacheItemRepository<T>  where T: CacheListItemBaseModel
    {
        private IRedisClient client;
        private IRedisTypedClient<T> clientApi;

        public abstract string ListName { get; }

        public RedisItemRepositoryBase()
        {

        }

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
            return string.Concat(ListName, ":", itemId);
        }

        public T AddItem(T cacheItem)
        {
            return ClientApi.Store(cacheItem);
        }

        public IEnumerable<T> GetByIds(IEnumerable<string> ids)
        {
            return ClientApi.GetByIds(ids);
        }

        public void RemoveItem(string id)
        {
            ClientApi.DeleteById(id);
        }

        public T UpdateItem(T cacheItem)
        {
            return ClientApi.Store(cacheItem);
        }
        public T GetItem(string id)
        {
            return ClientApi.GetById(id);
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
