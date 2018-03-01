using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using SFeed.Data.Infrastructure.RedisRepository;
using System;
using System.Collections.Generic;

namespace SFeed.Data.Infrastructure
{
    public abstract class RedisListRepositoryBase<T> : ICacheListRepository<T>, IDisposable
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
            client.Dispose();
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

        public void AddToList(string listKey, T item)
        {
            var listRef = GetAssociatedList(listKey);
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

        public bool ExistsInList(string listKey, T item)
        {
            var listRef = GetAssociatedList(listKey);
            return listRef.Contains(item);
        }

        public void DeleteList(string listKey)
        {
            clientApi.DeleteById(GetListName(listKey));
        }
    }
}
