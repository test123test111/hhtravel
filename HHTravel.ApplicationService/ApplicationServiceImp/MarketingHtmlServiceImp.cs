using HHTravel.CRM.Booking_Online.DataService;

namespace HHTravel.CRM.Booking_Online.ApplicationService.ApplicationServiceImp
{
    public class MarketingHtmlServiceImp : IMarketingHtmlService
    {
        public string GetMarketingHtml(string marketingHtmlLocation, bool useTestVersion = false)
        {
            return MarketingHtmlService.GetMarketingHtml(marketingHtmlLocation, useTestVersion);
        }
    }
}