using System.Web.Mvc;
using HHTravel.CRM.Booking_Online.Site.Models;

namespace HHTravel.CRM.Booking_Online.Site.Filter
{
    public class MetaDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;

            if (viewResult == null)
                return;

            // defaults - could load from settings / database / whereever

            var description = "Some description";
            var keywords = "demo, website, mvc";

            var model = viewResult.Model as IMetaDataModel;

            if (model != null)
            {
                description = model.Description;
                keywords = model.Keywords;
            }

            viewResult.ViewBag.Description = description;
            viewResult.ViewBag.Keywords = keywords;

            base.OnActionExecuted(filterContext);
        }
    }
}