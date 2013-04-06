using EFCachingProvider;
using EFCachingProvider.Caching;
using EFTracingProvider;
using HHTravel.CRM.Booking_Online.Infrastructure;

namespace RepositoryTest
{
    public class RepositoryTestBase
    {
        static RepositoryTestBase()
        {
            GlobalConfig.EnableEFTracing = true;
            GlobalConfig.EnableEFCaching = false;

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