using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.ApplicationService
{
    public interface IHtmlMetaService
    {
        HtmlMeta GetPropertyHtmlMeta(int propertyId);
    }
}