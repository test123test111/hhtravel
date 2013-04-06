using System.Web.Optimization;

namespace HHTravel.Site.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var styleBundle = new StyleBundle("~/Content/css").Include(
                "~/css/reset.css",
                "~/css/common.css",
                "~/css/layout.css"
                );
            bundles.Add(styleBundle);
        }
    }
}