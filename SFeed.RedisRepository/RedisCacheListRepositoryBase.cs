using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SFeed.Core.Infrastructue.Repository;
using SFeed.Core.Models.Caching;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Linq;

namespace SFeed.RedisRepository
{
    public abstract class RedisNamedCacheListRepository<T> : INamedCacheListRepository<T> where T: CacheListItemBaseModel
    {
        public abstract string ListName { get; set; }

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

        public void AddItem(string listName, T item)
        {
            var entryName = GetEntryName(listName, item.Id);
            ClientApi.SetValue(entryName, item);
        }

        public IEnumerable<T> GetList(string listName)
        {
            //max 1000
            var entryKeys = Client.ScanAllKeys(string.Concat(ListName,":", listName, ":*")).ToList();
            return ClientApi.GetValues(entryKeys);
        }

        public T GetItem(string listName, string itemId)
        {
            var entryName = GetEntryName(listName, itemId);
            return ClientApi.GetValue(entryName);
        }

        public void RemoveItem(string listName, string itemId)
        {
             var entryName = GetEntryName(listName, itemId);
             ClientApi.RemoveEntry(entryName);
        }

        public void RecreateList(string listName, IEnumerable<T> listItems)
        {
            DeleteList(listName);
            foreach (var listItem in listItems)
            {
                AddItem(listName, listItem);
            }
        }

        public void DeleteAllLists()
        {
            var entryKeys = Client.ScanAllKeys(string.Concat(ListName,":*")).ToList();
            ClientApi.DeleteByIds(entryKeys);
        }


        public void DeleteList(string listName)
        {
            var entryKeys = Client.ScanAllKeys(string.Concat(ListName, ":", listName, ":*")).ToList();
            ClientApi.DeleteByIds(entryKeys);
        }
    }
}
