namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class SecondLevelCacheModel
    {
        public SecondLevelCacheModel(int cacheHits, int cacheMisses, int cacheAdds) //long effectivePercentagePhysicalMemoryLimit, long effectivePrivateBytesLimit
        {
            this.CacheAdds = cacheAdds;
            this.CacheHits = cacheHits;
            this.CacheMisses = cacheMisses;
        }

        public int CacheAdds { get; private set; }

        public int CacheHits { get; private set; }

        public int CacheMisses { get; private set; }

        //public long EffectivePercentagePhysicalMemoryLimit { get; private set; }
        //public long EffectivePrivateBytesLimit { get; private set; }
    }
}