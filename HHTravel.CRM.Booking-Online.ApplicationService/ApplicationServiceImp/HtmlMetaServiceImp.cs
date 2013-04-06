using HHTravel.CRM.Booking_Online.DataService;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.ApplicationService.ApplicationServiceImp
{
    public class HtmlMetaServiceImp : IHtmlMetaService
    {
        public HtmlMeta GetPropertyHtmlMeta(int propertyId)
        {
            return HtmlMetaService.GetPropertyHTMLMeta(propertyId);
        }
    }
}