using System;
using System.Linq;
using System.Web.Mvc;
using HHTravel.Infrastructure.Crosscutting;
using HHTravel.Site.Models;

namespace HHTravel.Site.Controllers
{
    public class CalendarController : ControllerBase
    {
        /// <summary>
        /// 产品在日历上的可显示范围
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public ActionResult Index(string productNo, int? year, int? month)
        {
            CalendarModel calendarModel = null;

            if (!year.HasValue || !month.HasValue)
            {
                return Json(calendarModel, JsonRequestBehavior.AllowGet);
            }

            var p = ProductService.GetProduct(productNo);
            if (!p.DepartureDateList.Any())
            {
                return Json(calendarModel, JsonRequestBehavior.AllowGet);
            }

            // 产品的最早可出行日期
            DateTime firstDepartureDate = p.DepartureDateList.Min(dd => dd.Date);

            // 产品的最晚可出行日期
            DateTime lastDepartureDate = p.DepartureDateList.Max(dd => dd.Date);

            calendarModel = new CalendarModel(firstDepartureDate, lastDepartureDate, year.Value, month.Value);

            if (calendarModel.MonthModel == null)
            {
                return Json(calendarModel, JsonRequestBehavior.AllowGet);
            }

            foreach (var dayModel in calendarModel.MonthModel.Days)
            {
                var date = dayModel.Date;

                // 日历上日期在可出发日期内的
                var a = from pd in p.DepartureDateList
                        where date == pd.Date
                        select pd;
                if (a.Any())
                {
                    calendarModel.MonthModel.HasDepartureDate = true;

                    if (p.SetOffDateList.Contains(date))
                    {
                        dayModel.IsSetOff = true;
                    }
                    else
                    {
                        dayModel.Price = a.Min(pd => pd.Price);
                        dayModel.MinGroupNum = (p.TravelType == TravelType.TravelType1 || p.TravelType == TravelType.TravelType4) ? a.Min(pd => pd.MinGroupNum) : 0;
                    }
                }
            }

            return Json(calendarModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}