﻿using SFeed.Core.Models.Caching;
using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructue.Repository
{
    public interface ITypedCacheItemRepository<T> : IDisposable where T : CacheListItemBaseModel
    {
        T AddItem(T cacheItem);
        T GetItem(string id);
        IEnumerable<T> GetByIds(IEnumerable<string> ids);
        void RemoveItem(string id);
        T UpdateItem(T cacheItem);
    }
}
