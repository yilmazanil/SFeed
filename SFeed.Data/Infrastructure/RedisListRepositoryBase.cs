using System;
using System.Collections.Generic;

namespace SFeed.Data.Infrastructure
{
    public abstract class RedisListRepositoryBase<T, K> : ICacheListRepository<T, K>
    {
        protected abstract string ListPrefix { get; }

        public void Add(T key, K item)
        {
            using (var redis = RedisConnectionHelper.ClientManager.GetClient())
            {
                var followers = redis.As<K>();
                followers.Lists[ListPrefix + key].Add(item);
            }

        }

        public void Clear(T key)
        {
            using (var redis = RedisConnectionHelper.ClientManager.GetClient())
            {
                var followers = redis.As<K>();
                followers.Lists[ListPrefix + key].Clear();
            }
        }

        public void Refresh(T key, IEnumerable<K> itemCollection)
        {
            using (var redis = RedisConnectionHelper.ClientManager.GetClient())
            {
                var followerList = redis.As<K>();
                var redisList = followerList.Lists[ListPrefix + key];
                redisList.Clear();

                foreach (var item in itemCollection)
                {
                    redisList.Add(item);
                }

            }
        }

        public void Remove(T key, K item)
        {
            using (var redis = RedisConnectionHelper.ClientManager.GetClient())
            {
                var followerList = redis.As<K>();
                var listName = ListPrefix + key;
                followerList.Lists[listName].RemoveValue(item);
            }
        }

        public IEnumerable<K> Retrieve(T key)
        {
            using (var redis = RedisConnectionHelper.ClientManager.GetClient())
            {
                var followerList = redis.As<K>();
                return followerList.Lists[ListPrefix + key].GetAll();
            }
        }
    }
}
