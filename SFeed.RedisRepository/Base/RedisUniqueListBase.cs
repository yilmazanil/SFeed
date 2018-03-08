using System;
using System.Collections.Generic;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Linq;
using SFeed.Core.Infrastructure.Repository;

namespace SFeed.RedisRepository
{
    public abstract class RedisUniqueListBase<T> : ICacheChildListRepository<T>
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

        protected virtual string GetEntryName(string parentKey, string key)
        {
            return string.Concat(ListName, ":", parentKey , ":", key);
        }

        protected virtual string GetListItemSearchPattern(string parentKey)
        {
            return string.Concat(ListName, ":", parentKey, ":*");
        }

        protected List<string> GetListItemKeys(string parentKey)
        {
            var keySearchPattern = GetListItemSearchPattern(parentKey);
            return Client.ScanAllKeys(keySearchPattern).ToList();
        }


        public virtual IEnumerable<T> GetList(string parentKey)
        {
            //max 1000
            var keys = GetListItemKeys(parentKey);
            return ClientApi.GetValues(keys);
        }

        public virtual void RemoveItem(string parentKey, string key)
        {
             var entryName = GetEntryName(parentKey, key);
             ClientApi.RemoveEntry(entryName);
        }

       

        public void AddOrUpdateItem(string listKey, string key, T item)
        {
            var entryName = GetEntryName(listKey, key);
            ClientApi.SetValue(entryName, item);
        }

        public T GetItem(string listKey, string key)
        {
            var entryName = GetEntryName(listKey, key);
            return ClientApi.GetValue(entryName);
        }

        public void RecreateList(string listKey, Dictionary<string, T> listItems)
        {
            using (var transaction = Client.CreateTransaction())
            {
                ClearList(listKey);
                foreach (var listItem in listItems)
                {
                    AddOrUpdateItem(listKey, listItem.Key, listItem.Value);
                }
                transaction.Commit();
            }
        }

        public void ClearList(string listKey)
        {
            var keys = GetListItemKeys(listKey);

            Client.RemoveAll(keys);
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
