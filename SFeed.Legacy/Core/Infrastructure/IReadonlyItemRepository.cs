using System;
using System.Collections.Generic;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface IReadonlyItemRepository<T> : IDisposable
    {
        T GetItem(string id);
        IEnumerable<T> GetItems(IEnumerable<string> id);
    }
}
