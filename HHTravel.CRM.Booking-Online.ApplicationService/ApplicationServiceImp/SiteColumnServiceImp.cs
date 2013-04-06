using HHTravel.CRM.Booking_Online.DataService;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.ApplicationService.ApplicationServiceImp
{
    public class SiteColumnServiceImp : ISiteColumnService
    {
        public HtmlMeta GetHtmlMeta(SiteColumn siteColumn)
        {
            return SiteColumnService.GetHTMLMeta(siteColumn);
        }

        public ImageInfo GetTopImage(SiteColumn siteColumn)
        {
            return SiteColumnService.GetTopImage(siteColumn);
        }
    }
}