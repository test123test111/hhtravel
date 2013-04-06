using HHTravel.DataService;

namespace HHTravel.ApplicationService.ApplicationServiceImp
{
    public class MarketingHtmlServiceImp : IMarketingHtmlService
    {
        public string GetMarketingHtml(string marketingHtmlLocation, bool useTestVersion = false)
        {
            return MarketingHtmlService.GetMarketingHtml(marketingHtmlLocation, useTestVersion);
        }
    }
}