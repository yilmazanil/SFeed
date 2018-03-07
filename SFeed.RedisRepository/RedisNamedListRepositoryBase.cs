using System;
using System.Collections.Generic;
using SFeed.Core.Infrastructue.Repository;
using SFeed.Core.Models.Caching;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Linq;

namespace SFeed.RedisRepository
{
    public abstract class RedisNamedListRepositoryBase<T> : INamedCacheListRepository<T> where T: CacheListItemBaseModel
    {
        public abstract string ListName { get; }

        private IRedisClient client;
        private IRedisTypedClient<T> clientApi;

        protected IRedisClient Client
        {
            get {
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

        protected virtual string GetEntryName(string listId, string itemId)
        {
            return string.Concat(ListName, ":", listId , ":", itemId);
        }

        public virtual void AddOrUpdateItem(string listId, T item)
        { 
            var entryName = GetEntryName(listId, item.Id);
            ClientApi.SetValue(entryName, item);
        }

        public virtual IEnumerable<T> GetList(string listId)
        {
            //max 1000
            var entryKeys = Client.ScanAllKeys(string.Concat(ListName,":", listId, ":*")).ToList();
            return ClientApi.GetValues(entryKeys);
        }

        public virtual T GetItem(string listId, string itemId)
        {
            var entryName = GetEntryName(listId, itemId);
            return ClientApi.GetValue(entryName);
        }

        public virtual void RemoveItem(string listId, string itemId)
        {
             var entryName = GetEntryName(listId, itemId);
             ClientApi.RemoveEntry(entryName);
        }

        public virtual void RecreateList(string listId, IEnumerable<T> listItems)
        {
            DeleteList(listId);
            foreach (var listItem in listItems)
            {
                AddOrUpdateItem(listId, listItem);
            }
        }

        public virtual void DeleteList(string listId)
        {
            var entryKeys = Client.ScanAllKeys(string.Concat(ListName, ":", listId, ":*")).ToList();
            foreach (var entryKey in entryKeys)
            {
                ClientApi.RemoveEntry(entryKey);
            }
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
