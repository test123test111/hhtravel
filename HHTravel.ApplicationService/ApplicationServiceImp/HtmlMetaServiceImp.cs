using HHTravel.DataService;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.ApplicationService.ApplicationServiceImp
{
    public class HtmlMetaServiceImp : IHtmlMetaService
    {
        public HtmlMeta GetPropertyHtmlMeta(int propertyId)
        {
            return HtmlMetaService.GetPropertyHTMLMeta(propertyId);
        }
    }
}