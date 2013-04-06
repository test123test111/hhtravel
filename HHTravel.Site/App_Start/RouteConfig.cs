using System.Web.Mvc;
using System.Web.Routing;

namespace HHTravel.CRM.Booking_Online.Site.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    "ProductList",
            //    "{controller}/{action}/{searchType}/{searchCode}",
            //    new { controller = "Product", action = "Search" }
            //);
            routes.MapRoute(
                "Sitemap",
                "sitemap.xml",
                new { controller = "Home", action = "SitemapXml" }
            );
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
                "NoResults", // Route name
                "Product/NoResults", // URL with parameters
                new { controller = "Product", action = "NoResults" }
            );
            routes.MapRoute(
                "Details", // Route name
                "Product/{productNo}", // URL with parameters
                new { controller = "ProductDetails", action = "Index" }
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
                new { controller = "ProductDetails", }
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
    }
}