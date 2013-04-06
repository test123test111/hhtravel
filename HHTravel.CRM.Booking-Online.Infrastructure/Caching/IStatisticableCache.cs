using EFCachingProvider.Caching;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Caching
{
    public interface IStatisticableCache : ICache
    {
        int CacheHits { get; }

        int CacheItemAdds { get; }

        int CacheItemInvalidations { get; }

        int CacheMisses { get; }
    }
}