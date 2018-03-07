using SFeed.Core.Models.Caching;

namespace SFeed.Core.Infrastructure.Repository
{
    public interface ICacheItemRepository<T> where T: CacheListItemBaseModel
    {
        void AddItem(T item);
        T GetItem(T item);
        void DeleteItem(string itemId);
        void UpdateItem(T item);
    }
}
