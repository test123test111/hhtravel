using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HHTravel.CRM.Booking_Online.Site
{
    public static class UrlExtensions
    {
        public static string Index(this UrlHelper UrlHelper)
        {
            return UrlHelper.Action("Index", "Home");
        }

        public static string AbsoluteIndex(this UrlHelper UrlHelper)
        {
            return UrlHelper.AbsoluteAction("Index", "Home", null);
        }

        public static string IndexCn(this UrlHelper UrlHelper)
        {
            return UrlHelper.Action("IndexCn", "Home");
        }

        public static string AbsoluteIndexCn(this UrlHelper UrlHelper)
        {
            return UrlHelper.AbsoluteAction("IndexCn", "Home", null);
        }

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
    }
}