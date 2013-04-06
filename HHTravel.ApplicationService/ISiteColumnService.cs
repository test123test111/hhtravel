using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.ApplicationService
{
    public interface ISiteColumnService
    {
        HtmlMeta GetHtmlMeta(SiteColumn siteColumn);

        ImageInfo GetTopImage(SiteColumn siteColumn);
    }
}