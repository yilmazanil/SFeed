using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using SFeed.Core.Infrastructue.Repository;
using System;
using System.Collections.Generic;

namespace SFeed.RedisRepository
{
    public abstract class RedisListRepositoryBase<T> : ICacheListRepository<T>
    {
        protected abstract string ListPrefix { get; }

        private IRedisClient client;

        protected IRedisTypedClient<T> clientApi;

        public RedisListRepositoryBase()
        {
            client = RedisConnectionProvider.GetClient();
            clientApi = client.As<T>();
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


        protected virtual string GetListName(string key)
        {
            return string.Concat(ListPrefix, ":", key);
        }

        protected IRedisList<T> GetAssociatedList(string listKey)
        {
            var listName = GetListName(listKey);
            return clientApi.Lists[listName];
        }

        public void AppendToList(string listKey, T item)
        {
            var listRef = GetAssociatedList(listKey);
            listRef.RemoveValue(item);
            listRef.Add(item);
        }

        public IEnumerable<T> GetList(string listKey)
        {
            var listRef = GetAssociatedList(listKey);
            return listRef.GetAll();
        }

        public void RecreateList(string listKey, IEnumerable<T> listItems)
        {

            var listRef = GetAssociatedList(listKey);
            listRef.Clear();

            foreach (var item in listItems)
            {
                listRef.Add(item);
            }
        }

        public void RemoveFromList(string listKey, T item)
        {
            var listRef = GetAssociatedList(listKey);
            listRef.RemoveValue(item);
        }

        public void ClearList(string listKey)
        {
            var listRef = GetAssociatedList(listKey);
            listRef.Clear();
        }

        public void DeleteList(string listKey)
        {
            clientApi.DeleteById(GetListName(listKey));
        }

        public void PrependToList(string listKey, T item)
        {
            var listRef = GetAssociatedList(listKey);
            listRef.RemoveValue(item);
            listRef.Prepend(item);
        }
    }
}
