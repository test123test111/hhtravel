using HHTravel.DataService;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.ApplicationService.ApplicationServiceImp
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