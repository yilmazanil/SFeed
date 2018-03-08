using ServiceStack.Redis;
using SFeed.Core.Infrastructure.Repository;
using System;

namespace SFeed.RedisRepository.Base
{
    public abstract class RedisCounterBase : ICacheItemCounter
    {
        public abstract string counterPrefix { get; }


        private IRedisClient client;

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

        private string GetEntryName(string key)
        {
            return string.Concat(counterPrefix, key);
        }

        public void Increment(string key)
        {
            var entryKey = GetEntryName(key);
            Client.Increment(key, 1);
        }


        public void Decrement(string key)
        {
            var entryKey = GetEntryName(key);
            var value = Client.GetValue(entryKey);

            if (!string.IsNullOrWhiteSpace(value) && Convert.ToInt32(value) > 0)
            {
                Client.Decrement(key, 1);
            }
        }

        public int GetValue(string key)
        {
            var entryKey = GetEntryName(key);
            var value = Client.GetValue(entryKey);

            return !string.IsNullOrWhiteSpace(value) ? Convert.ToInt32(value) : 0;
        }

        public void SetValue(string key, int value)
        {
            var entryKey = GetEntryName(key);
            Client.SetValue(key, Convert.ToString(value));
        }

        public void Remove(string key)
        {
            var entryKey = GetEntryName(key);
            Client.Remove(key);
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
