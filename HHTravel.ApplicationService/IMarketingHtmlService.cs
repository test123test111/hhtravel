namespace HHTravel.CRM.Booking_Online.ApplicationService
{
    public interface IMarketingHtmlService
    {
        string GetMarketingHtml(string marketingHtmlLocation, bool useTestVersion = false);
    }
}