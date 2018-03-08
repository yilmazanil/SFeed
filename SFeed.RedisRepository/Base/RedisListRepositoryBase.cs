using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using SFeed.Core.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SFeed.RedisRepository.Base
{
    public abstract class RedisListRepositoryBase<T> : ICacheListRepository<T>
    {
        public abstract string ListName { get; }

        private IRedisClient client;
        private IRedisTypedClient<T> clientApi;

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

        protected virtual string GetEntryKey(string listKey)
        {
            return string.Concat(ListName, ":", listKey);
        }

        public void AddItem(string listKey, T item)
        {
            var entryKey = GetEntryKey(listKey);
            ClientApi.Lists[entryKey].Add(item);
        }

        public void ClearList(string listKey)
        {
            var entryKey = GetEntryKey(listKey);
            ClientApi.Lists[entryKey].Clear();
        }


        //Might run slow
        public T GetItem(string listKey , Expression<Func<T, bool>> where)
        {
            var entryKey = GetEntryKey(listKey);
            var items = ClientApi.Lists[entryKey].GetAll().AsQueryable();
            if (items != null)
            {
                return items.FirstOrDefault(where);
            }
            return default(T);
        }

        public IEnumerable<T> GetList(string listKey)
        {
            var entryKey = GetEntryKey(listKey);
            return ClientApi.Lists[entryKey].GetAll();
        }

        public void RecreateList(string listKey, IEnumerable<T> listItems)
        {
            ClearList(listKey);
            var entryKey = GetEntryKey(listKey);
            ClientApi.Lists[entryKey].AddRange(listItems);
        }

        public void RemoveItem(string listKey, T item)
        {
            var entryKey = GetEntryKey(listKey);
            ClientApi.Lists[entryKey].Remove(item);
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
