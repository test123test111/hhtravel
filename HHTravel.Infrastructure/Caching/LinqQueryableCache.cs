using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace HHTravel.Infrastructure.Caching
{
    public class LinqQueryableCache : IQueryableCache
    {
        private static IQueryableCache s_cacheInstance;
        private static IList<string> s_list;
        private static object s_lockObject = new object();
        private HttpContext _httpContext;

        private LinqQueryableCache(HttpContext httpContext)
        {
            this._httpContext = httpContext;
            s_list = new List<string>();
        }

        public static IQueryableCache Instance
        {
            get
            {
                if (s_cacheInstance == null)
                {
                    lock (s_lockObject)
                    {
                        if (s_cacheInstance == null)
                            s_cacheInstance = new LinqQueryableCache(null);
                    }
                }

                return s_cacheInstance;
            }
        }

        private Cache HttpCache
        {
            get
            {
                if (this._httpContext != null)
                {
                    return this._httpContext.Cache;
                }

                var context = HttpContext.Current;
                if (context == null)
                {
                    throw new InvalidOperationException("Unable to determine HTTP context.");
                }

                return context.Cache;
            }
        }

        public object Get(string key)
        {
            object o = null;
            if (s_list.Any(i => i == key))
            {
                o = this.HttpCache.Get(key);
            }
            return o;
        }

        public IDictionary<string, object> GetObjectList(string key)
        {
            IDictionary<string, object> dict = new Dictionary<string, object>();
            var query = from k in s_list
                        where string.IsNullOrWhiteSpace(key) || k.Contains(key)
                        select new
                        {
                            Key = k,
                            Value = this.HttpCache.Get(k)
                        };
            if (query.Any())
            {
                dict = query.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
            return dict;
        }

        public void Insert(string key, object value, System.Web.Caching.CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, System.Web.Caching.CacheItemPriority priority, System.Web.Caching.CacheItemRemovedCallback onRemoveCallback)
        {
            if (!s_list.Contains(key))
            {
                s_list.Add(key);
            }
            this.HttpCache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }

        public void Remove(string key)
        {
            s_list.Remove(key);
            this.HttpCache.Remove(key);
        }
    }
}