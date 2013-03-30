using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HHTravel.CRM.Booking_Online.Site.Models;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.Site.Controllers
{
    public class CalendarController : BaseController
    {
        //
        // GET: /Calendar/
        //[OutputCache(Duration = 1800)]
        /// <summary>
        /// 产品在日历上的可显示范围
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public ActionResult Index(string productNo, int year, int month)
        {
            var p = ProductService.GetProduct(productNo);
            var allRoomClasses = ProductService.GetRoomClasses(p.ProductId, false);
            // 产品的最早可出行日期
            DateTime firstDepartureDate = DateTime.Now.Date;
            // 产品的最晚可出行日期
            DateTime lastDepartureDate = allRoomClasses.Max(new Func<RoomClass, DateTime>(bci => bci.ExpireDate));

            // 验证：
            // 该产品的最晚可出行日期不能小于最早可出行日期
            // 传入的年/月，需在该产品的（最早可出行日期~最晚可出行日期）之间
            if (lastDepartureDate < firstDepartureDate ||
                year < firstDepartureDate.Year || (year == firstDepartureDate.Year && month < firstDepartureDate.Month)
                || (year > lastDepartureDate.Year) || (year == lastDepartureDate.Year && month > lastDepartureDate.Month))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            // 创建月份对应的日历视图模型
            MonthModel monthModel = new MonthModel(year, month);

            var monthBeginDate = monthModel.Days[0].Date;
            var monthEndDate = monthModel.Days[monthModel.Days.Count - 1].Date;

            // 团队游需要显示最小成行人数
            int minPersonNum = 0;
            if (p.TravelType == TravelType.团队游)
            {
                minPersonNum = ProductService.GetMinPersonNumList(p.ProductId);
            }

            var delta = monthEndDate - monthBeginDate;
            for (int i = 0; i <= delta.TotalDays; i++)
            {
                var date = monthBeginDate.AddDays(i);
                // 考虑上产品的提前预定的日期
                if (p.AdvanceDays.HasValue)
                {
                    var firstAvailableDate = firstDepartureDate.AddDays(p.AdvanceDays.Value);
                    // 早于可预订日期的排除掉
                    if (date < firstAvailableDate)
                    {
                        continue;
                    }
                }

                // 产品的房型价格在价格有效期内的
                var a = from bci in allRoomClasses
                        where date >= bci.EffectDate && date <= bci.ExpireDate
                        select bci;
                if (a.Any())
                {
                    var price = a.Min(bci => bci.Price);
                    // 没有出发日期的排除掉
                    if (p.DepartureDateList.Contains(date))
                    {
                        var dayModel = monthModel.Days.Find(dm => dm.Date == date);
                        dayModel.MinPersonNum = minPersonNum;
                        dayModel.Price = price;
                    }
                }
            }

            return Json(monthModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}
