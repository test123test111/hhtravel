using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFCachingProvider;
using EFTracingProvider;
using EFCachingProvider.Caching;
using HHTravel.CRM.Booking_Online.DataAccess;

namespace DataAccessTest
{
    public class RepositoryTestBase
    {
        static RepositoryTestBase()
        {
            DbContextFactory.EnableTracing = true;
            DbContextFactory.EnableCaching = true;
            // 只打印SQL语句
            //DbContextFactory.Log = new ActionTextWriter(info => Console.WriteLine(info));
            EFCachingProviderConfiguration.DefaultCache = new InMemoryCache();
            EFCachingProviderConfiguration.DefaultCachingPolicy = CachingPolicy.CacheAll;
            // 会打印"1 Running ExecuteReader:"和执行时间
            EFTracingProviderConfiguration.LogToConsole = true;
            // todo: 需要修复EFCachingProvider中，注入的TextWriter（通过Log属性）不输出执行时间的问题
        }
    }
}
