// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.Caching;
using EFCachingProvider.Caching;
using HHTravel.Base.Common.Framework.Logging;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Caching
{
    /// <summary>
    /// Implementation of <see cref="ICache"/> which works with ASP.NET cache object.
    /// </summary>
    public sealed class EFCache : IStatisticableCache
    {
        private const string DependentEntitySetPrefix = "dependent_entity_set_";
        private static EFCache _cacheInstance;
        private static ConcurrentDictionary<string, string> s_dict = new ConcurrentDictionary<string, string>();

        private static object s_lockObject = new object();
        private int _cacheAdds = 0;
        private int _cacheHits = 0;
        private int _cacheItemInvalidations = 0;
        private int _cacheMisses = 0;

        /// <summary>
        /// Initializes a new instance of the EFCache class.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        private EFCache()
        {
        }

        public static EFCache Instance
        {
            get
            {
                if (_cacheInstance == null)
                {
                    lock (s_lockObject)
                    {
                        if (_cacheInstance == null)
                            _cacheInstance = new EFCache();
                    }
                }

                return _cacheInstance;
            }
        }

        public int CacheHits
        {
            get { return this._cacheHits; }
        }

        public int CacheItemAdds
        {
            get { return this._cacheAdds; }
        }

        public int CacheItemInvalidations
        {
            get { return this._cacheItemInvalidations; }
        }

        public int CacheMisses
        {
            get { return this._cacheMisses; }
        }

        private IQueryableCache HttpCache
        {
            get
            {
                return LinqQueryableCache.Instance;
            }
        }

        public IEnumerable<EFCacheModel> Find(string key)
        {
            IEnumerable<EFCacheModel> d = new List<EFCacheModel>();

            // 获取sql语句包含keyword 的缓存项的键
            var keys = from kvp in s_dict
                       where string.IsNullOrWhiteSpace(key) || kvp.Value.Contains(key)
                       select kvp.Key;

            if (keys.Any())
            {
                foreach (var k in keys)
                {
                    var a = from kvp in this.HttpCache.GetObjectList(k)
                            select new EFCacheModel
                            {
                                Key = kvp.Key,
                                SQL = s_dict[kvp.Key],
                                Value = kvp.Value,
                            };
                    d = d.Concat(a);
                }
            }
            else
            {
                d = from kvp in this.HttpCache.GetObjectList(key)
                    select new EFCacheModel
                    {
                        Key = kvp.Key,
                        Value = kvp.Value,
                    };
            }
            return d;
        }

        /// <summary>
        /// Tries to the get entry by key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The retrieved value.</param>
        /// <returns>
        /// A value of <c>true</c> if entry was found in the cache, <c>false</c> otherwise.
        /// </returns>
        public bool GetItem(string key, out object value)
        {
            var newkey = GetCacheKey(key);
            value = this.HttpCache.Get(newkey);

            if (value != null)
            {
                Interlocked.Increment(ref this._cacheHits);
                return true;
            }
            else
            {
                Interlocked.Increment(ref this._cacheMisses);
                return false;
            }
        }

        /// <summary>
        /// Invalidates cache entry with a given key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public void InvalidateItem(string key)
        {
            key = GetCacheKey(key);
            this.HttpCache.Remove(key);
        }

        /// <summary>
        /// Invalidates all cache entries which are dependent on any of the specified entity sets.
        /// </summary>
        /// <param name="entitySets">The entity sets.</param>
        public void InvalidateSets(IEnumerable<string> entitySets)
        {
            foreach (string entitySet in entitySets)
            {
                this.HttpCache.Remove(DependentEntitySetPrefix + entitySet);
            }
        }

        /// <summary>
        /// Adds the specified entry to the cache.
        /// </summary>
        /// <param name="key">The entry key.</param>
        /// <param name="value">The entry value.</param>
        /// <param name="dependentEntitySets">The list of dependent entity sets.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        public void PutItem(string key, object value, IEnumerable<string> dependentEntitySets, TimeSpan slidingExpiration, DateTime absoluteExpiration)
        {
            var newkey = GetCacheKey(key);
            var cache = this.HttpCache;

            foreach (var entitySet in dependentEntitySets)
            {
                this.EnsureEntryExists(DependentEntitySetPrefix + entitySet);
            }

            try
            {
                CacheDependency cd = new CacheDependency(new string[0], dependentEntitySets.Select(c => DependentEntitySetPrefix + c).ToArray());
                cache.Insert(newkey, value, cd, absoluteExpiration, slidingExpiration, CacheItemPriority.Normal, (k, v, reason) =>
                {
                    string val;
                    s_dict.TryRemove(k, out val);
                });

                Interlocked.Increment(ref this._cacheAdds);
            }
            catch (Exception e)
            {
                // there's a possibility that one of the dependencies has been evicted by another thread
                // in this case just don't put this item in the cache
                SysLog.WriteException("预设忽略的异常", e,
                    string.Format("Key: {0} has inserted. Original Key: {1}, time: {2}", newkey, key, DateTime.Now));
            }
        }

        /// <summary>
        /// Hashes the query to produce cache key..
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>Hashed query which becomes a cache key.</returns>
        private static string GetCacheKey(string query)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(query);
            byte[] hash = MD5.Create().ComputeHash(bytes);
            string hashString = Convert.ToBase64String(hash);

            s_dict.TryAdd(hashString, query);

            return hashString;
        }

        private void EnsureEntryExists(string key)
        {
            var cache = this.HttpCache;

            if (cache.Get(key) == null)
            {
                try
                {
                    cache.Insert(key, key, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                }
                catch (Exception ex)
                {
                    // ignore exceptions.
                    SysLog.WriteException("预设忽略的异常", ex, string.Format("Key: {0}", key));
                }
            }
        }
    }

    public class EFCacheModel
    {
        public string Key { get; set; }

        public string SQL { get; set; }

        public object Value { get; set; }
    }
}