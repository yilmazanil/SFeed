using System;
using System.Collections.Generic;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Linq;
using SFeed.Core.Infrastructure.Repository;

namespace SFeed.RedisRepository
{
    public abstract class RedisUniqueListBase<T> : ICacheUniqueListRepository<T>
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

        protected virtual string GetListItemSearchPattern(string listId)
        {
            return string.Concat(ListName, ":", listId, ":*");
        }

        protected List<string> GetListItemKeys(string listId)
        {
            var keySearchPattern = GetListItemSearchPattern(listId);
            return Client.ScanAllKeys(keySearchPattern).ToList();
        }


        public virtual IEnumerable<T> GetList(string listId)
        {
            //max 1000
            var keys = GetListItemKeys(listId);
            return ClientApi.GetValues(keys);
        }

        public virtual void RemoveItem(string listId, string itemId)
        {
             var entryName = GetEntryName(listId, itemId);
             ClientApi.RemoveEntry(entryName);
        }

       

        public void AddOrUpdateItem(string listKey, string itemId, T item)
        {
            var entryName = GetEntryName(listKey, itemId);
            ClientApi.SetValue(entryName, item);
        }

        public T GetItem(string listKey, string itemId)
        {
            var entryName = GetEntryName(listKey, itemId);
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
