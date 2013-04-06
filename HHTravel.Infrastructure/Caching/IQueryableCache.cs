using System;
using System.Collections.Generic;
using System.Linq;

namespace HHTravel.Infrastructure.Caching
{
    public interface IQueryableCache
    {
        object Get(string key);

        IDictionary<string, object> GetObjectList(string key);

        void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback);

        void Remove(string key);
    }
}