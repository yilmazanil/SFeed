namespace SFeed.Core.Infrastructure.Repository
{
    public interface ICacheFixedListRepository<T> : ICacheListRepository<T>
    {
        int ListSize { get; set; }
    }
}
