using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.DataAccess;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.Infrastructure;

namespace HHTravel.CRM.Booking_Online.DataService
{
    /// <summary>
    /// 营销内容服务
    /// </summary>
    public class MarketingHtmlService
    {
        public static string GetMarketingHtml(string marketingHtmlLocation, bool useTestVersion = false)
        {
            string html = "";
            using (var cxt = DbContextFactory.Create<CustomerDbEntities>())
            {
                var query = from mh in cxt.Mkt_Html
                            where mh.Location == marketingHtmlLocation
                            select mh;
                var r = query.FirstOrDefault();
                if (r != null)
                {
                    html = useTestVersion ? r.HtmlTest : r.Html;
                }
            }
            return html;
        }
    }
}