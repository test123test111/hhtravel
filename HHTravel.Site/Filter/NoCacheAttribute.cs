using System.Web;
using System.Web.Mvc;

namespace HHTravel.Site.Filter
{
    // http://stackoverflow.com/questions/264216/getjson-returning-cached-data-in-ie8
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
    }
}