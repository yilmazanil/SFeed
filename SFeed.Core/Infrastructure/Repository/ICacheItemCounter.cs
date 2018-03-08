using System;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface ICacheItemCounter : IDisposable
    {
        void Increment(string key);
        void Decrement(string key);
        void Remove(string key);
        void SetValue(string key, int value);
        int GetValue(string key);
    }
}
