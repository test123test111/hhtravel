using System;
using System.Web.Mvc;

namespace HHTravel.Site.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult ServerError()
        {
            Exception ex = Server.GetLastError();
            ViewBag.Exception = ex;
            return View();
        }
    }
}