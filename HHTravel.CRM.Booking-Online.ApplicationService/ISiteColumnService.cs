using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.ApplicationService
{
    public interface ISiteColumnService
    {
        HtmlMeta GetHtmlMeta(SiteColumn siteColumn);

        ImageInfo GetTopImage(SiteColumn siteColumn);
    }
}