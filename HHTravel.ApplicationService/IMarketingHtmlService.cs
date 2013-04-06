namespace HHTravel.ApplicationService
{
    public interface IMarketingHtmlService
    {
        string GetMarketingHtml(string marketingHtmlLocation, bool useTestVersion = false);
    }
}