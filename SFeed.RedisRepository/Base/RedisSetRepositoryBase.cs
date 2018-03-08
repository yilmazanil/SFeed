//using ServiceStack.Redis;
//using ServiceStack.Redis.Generic;
//using SFeed.Core.Infrastructue.Repository;
//using System;
//using System.Collections.Generic;

//namespace SFeed.RedisRepository
//{
//    public abstract class RedisSetRepositoryBase : ICacheSetRepository
//    {
//        private IRedisClient client;

//        public abstract string SetName { get; }

//        protected IRedisClient Client
//        {
//            get
//            {
//                if (client == null)
//                {
//                    client = RedisConnectionProvider.GetClient();
//                }
//                return client;
//            }
//        }

//        protected virtual string GetEntryName(string itemId)
//        {
//            return string.Concat("urn:" + SetName, ":", itemId);
//        }

//        public bool Contains(string setId, string value)
//        {
//            var setName = GetEntryName(setId);
//            return Client.Sets[setName].Contains(value);
//        }

//        public void RemoveItem(string setId, string value)
//        {
//            var setName = GetEntryName(setId);
//            var set = Client.Sets[setName].Remove(value);
//        }

//        public void AddItem(string setId, string item)
//        {
//            var setName = GetEntryName(setId);
//            Client.AddItemToSet(setName, item);
//        }

//        public IEnumerable<string> GetItems(string setId)
//        {
//            var setName = GetEntryName(setId);
//            return Client.GetAllItemsFromSet(setName);
//        }

//        public void Clear(string setId)
//        {
//            var setName = GetEntryName(setId);
//             Client.Sets[setName].Clear();
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                if (client != null)
//                {
//                    client.Dispose();
//                }

//            }
//        }
//    }
//}
