using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using SFeed.Core.Infrastructue.Repository;
using System;
using System.Collections.Generic;

namespace SFeed.RedisRepository
{
    public abstract class RedisListRepositoryBase<T> : INamedCacheListRepository<T>
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




        protected virtual IRedisList<T> GetAssociatedList(string listKey)
        {
            var listName = GetListName(listKey);
            return clientApi.Lists[listName];
        }

        public virtual void AppendToList(string listKey, T item)
        {
            var listRef = GetAssociatedList(listKey);
            listRef.RemoveValue(item);
            listRef.Add(item);
        }

        public virtual IEnumerable<T> GetList(string listKey)
        {
            var listRef = GetAssociatedList(listKey);
            return listRef.GetAll();
        }

        public virtual void RecreateList(string listKey, IEnumerable<T> listItems)
        {

            var listRef = GetAssociatedList(listKey);
            listRef.Clear();

            foreach (var item in listItems)
            {
                listRef.Add(item);
            }
        }

        public virtual void RemoveFromList(string listKey, T item)
        {
            var listRef = GetAssociatedList(listKey);
            listRef.RemoveValue(item);
        }

        public virtual void ClearList(string listKey)
        {
            var listRef = GetAssociatedList(listKey);
            listRef.Clear();
        }

        public virtual void DeleteList(string listKey)
        {
            clientApi.DeleteById(GetListName(listKey));
        }

        public virtual void PrependToList(string listKey, T item)
        {
            var listRef = GetAssociatedList(listKey);
            listRef.RemoveValue(item);
            listRef.Prepend(item);
        }
    }
}
