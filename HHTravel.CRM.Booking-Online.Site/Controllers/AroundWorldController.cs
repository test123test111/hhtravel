using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HHTravel.CRM.Booking_Online.Site.Models;

namespace HHTravel.CRM.Booking_Online.Site.Controllers
{
    public class AroundWorldController : Controller
    {
        //
        // GET: /Around/
        public ActionResult Index()
        {
            var model = new TopNavModel
            {
                TopImageSrc = "~/images/banner_photo/FRN0000009473.jpg",
                BreadcrumbsText = "环游世界",
                BreadcrumbsText2 = "",
                BreadcrumbsUrl = "~/Home/AroundWorld",
            };
            return View("~/Views/Home/AroundWorld.cshtml", model);
        }      
    }
}
