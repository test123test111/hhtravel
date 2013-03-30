using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EFCachingProvider;
using HHTravel.Base.Common.Web;
using EFCachingProvider.Caching;
using HHTravel.CRM.Booking_Online.Business;
using System.Configuration;

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

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    "ProductList",
            //    "{controller}/{action}/{searchType}/{searchCode}",
            //    new { controller = "Product", action = "Search" }
            //);
            routes.MapRoute(
                "FindBy",
                "{controller}/{propertyType}/{propertyValue}/{childValue}",
                new { controller = "Product", action = "FindBy", childValue = UrlParameter.Optional },
                new { propertyType = @"\d+", propertyValue = @"\d+" }
            );
            routes.MapRoute(
                "FindBy2",
                "{controller}/FindBy",
                new { controller = "Product", action = "FindBy" }
            );
            routes.MapRoute(
                "Search", // Route name
                "Product/Search", // URL with parameters
                new { controller = "Product", action = "Search" }
            );
            routes.MapRoute(
                "Details", // Route name
                "Product/{productNo}", // URL with parameters
                new { controller = "Product", action = "Details" }
            );
            routes.MapRoute(
                "OrderOthers", // Route name
                "Order/{action}", // URL with parameters
                new { controller = "Order", action = "Index2" }
            );
            routes.MapRoute(
                "Order", // Route name
                "Order/{productNo}/{date}", // URL with parameters
                new { controller = "Order", action = "Index", date = UrlParameter.Optional },
                new { productNo = @"\d{3,}$" }
            );
            routes.MapRoute(
                "ProductOthers", // Route name
                "Product/{action}/{productNo}", // URL with parameters
                new { controller = "Product", }
            );
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            //routes.MapRoute(
            //    "Unknown", // Route name
            //    "{about}", // URL with parameters
            //    new { controller = "Home", action = "About" } // Parameter defaults
            //);

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            EFCachingProviderConfiguration.DefaultCache = new AspNetEFCache();
            EFCachingProviderConfiguration.DefaultCachingPolicy = CachingPolicy.CacheAll;

            bool mock = false;
            var a = ConfigurationManager.AppSettings["MockRepository"];
            if (a != null && string.Equals(a, bool.TrueString))
            {
                mock = true;
            }
            new HHTravel.CRM.Booking_Online.Business.ApplicationServiceImp.RepositoryServiceImp().Register(mock);
        }
    }
}