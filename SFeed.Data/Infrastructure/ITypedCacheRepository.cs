﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace SFeed.Data.Infrastructure
{
    public interface ITypedCacheRepository<T> : IDisposable where T : class
    {
        T AddItem(T cacheItem);
        T GetItem(object id);
        IEnumerable<T> GetByIds(IEnumerable<object> ids);
        void RemoveItem(object id);
    }
}
