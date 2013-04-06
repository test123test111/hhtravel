using EFCachingProvider.Caching;

namespace HHTravel.Infrastructure.Caching
{
    public interface IStatisticableCache : ICache
    {
        int CacheHits { get; }

        int CacheItemAdds { get; }

        int CacheItemInvalidations { get; }

        int CacheMisses { get; }
    }
}