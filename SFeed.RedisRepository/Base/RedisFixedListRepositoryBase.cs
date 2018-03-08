using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using SFeed.Core.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SFeed.RedisRepository.Base
{
    public abstract class RedisFixedListRepositoryBase<T> : ICacheFixedListRepository<T>
    {
        public abstract string ListName { get; }

        public abstract int ListSize { get; set; }

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

        public void AppendItem(string listKey, T item)
        {
            var entryKey = GetEntryKey(listKey);
            var list = ClientApi.Lists[entryKey];
            if (list.Count >= ListSize)
            {
                list.Trim(0, ListSize - 1);
            }
            list.Append(item);
        }

        public void PrependItem(string listKey, T item)
        {
            var entryKey = GetEntryKey(listKey);
            var list = ClientApi.Lists[entryKey];
            if (list.Count >= ListSize)
            {
                list.Trim(1, ListSize);
            }
            list.Prepend(item);
        }


        public bool UpdateItem(string listKey, T item)
        { 
            var entryKey = GetEntryKey(listKey);
            var list = ClientApi.Lists[entryKey];

            var itemIndex = list.IndexOf(item);
            if (itemIndex > -1)
            {
                list.Remove(item);
                list.Insert(itemIndex, item);
                return true;
            }
            return false;
        }

        public bool UpdateItem(string listKey, Predicate<T> where, T item)
        {
            var entryKey = GetEntryKey(listKey);
            var listRef = ClientApi.Lists[entryKey];
            var list = listRef.ToList();

            var itemIndex = list.FindIndex(where);
            var existingItem = list[itemIndex];
            if (itemIndex > -1)
            {
                listRef.Remove(existingItem);
                list.Insert(itemIndex, item);
                return true;
            }
            return false;
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
