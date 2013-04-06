using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EFCachingProvider;
using HHTravel.CRM.Booking_Online.Infrastructure;
using HHTravel.CRM.Booking_Online.Infrastructure.Caching;
using HHTravel.CRM.Booking_Online.Site.App_Start;

namespace HHTravel.CRM.Booking_Online.Site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HHTravel.CRM.Booking_Online.Site.Filter.ExceptionFilter());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleTable.EnableOptimizations = true;
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            EFCachingProviderConfiguration.DefaultCache = EFCache.Instance;
            EFCachingProviderConfiguration.DefaultCachingPolicy = new EFCachingPolicy();

            bool mock = GlobalConfig.MockRepository;
            new HHTravel.CRM.Booking_Online.ApplicationService.ApplicationServiceImp.RepositoryServiceImp().Register(mock);
        }
    }
}