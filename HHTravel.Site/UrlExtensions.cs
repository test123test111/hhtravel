using System;
using System.Web.Mvc;

namespace HHTravel.CRM.Booking_Online.Site
{
    public static class UrlExtensions
    {
        public static string AbsoluteAction(this UrlHelper UrlHelper, string actionName, string controllerName, object routeValues)
        {
            string protocol = UrlHelper.RequestContext.HttpContext.Request.Url.Scheme;
            return UrlHelper.Action(actionName, controllerName, routeValues, protocol);
        }

        public static string AbsoluteContent(this UrlHelper UrlHelper, string contentPath)
        {
            string left = UrlHelper.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
            return left + UrlHelper.Content(contentPath);
        }

        public static string AbsoluteIndex(this UrlHelper UrlHelper)
        {
            return UrlHelper.AbsoluteAction("Index", "Home", null);
        }

        public static string Index(this UrlHelper UrlHelper)
        {
            return UrlHelper.Action("Index", "Home");
        }

        public static string Product(this UrlHelper UrlHelper, string productNo)
        {
            return UrlHelper.AbsoluteAction("Index", "ProductDetails", new { ProductNo = productNo });
        }
    }
}