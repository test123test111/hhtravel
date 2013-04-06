using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.Site.Models;

namespace HHTravel.CRM.Booking_Online.Site.Controllers
{
    public class ProductDetailsController : ProductControllerBase
    {
        public ActionResult Hotels(string productNo)
        {
            var product = ProductService.GetProduct(productNo);
            var hotels = ProductService.GetHotels(product.ProductId);
            return View(hotels);
        }

        /// <summary>
        /// 日历|房型航班 信息
        /// </summary>
        /// <param name="productNo"></param>
        /// <param name="date"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public ActionResult CalendarRoomFlight(string productNo,
            string date, int? year, int? month, int? step)
        {
            ProductModel prodModel;
            if (!year.HasValue || !month.HasValue)
            {
                return null;
            }

            step = step ?? 2;   // 默认展示第二步
            var p = ProductService.GetProduct(productNo);
            prodModel = new ProductModel(p);

            DepartureCity? departureCity;
            if (p != null)
            {
                departureCity = DepartureCity.Parse(p.DepartureCity);
            }
            else
            {
                departureCity = CookieManager.DepartureCity;
            }
            BuildProductTopNavModelAndMeta(departureCity, p);

            ViewBag.Tab = 3;
            ViewBag.Step = step;

            DateTime selectedDepartureDate;
            bool pass = DateTime.TryParse(date, out selectedDepartureDate);

            if (step.Value == 2)
            {
                // 如果入参日期无效
                if (!pass)
                {
                    ViewBag.Step = 1;
                }
                else
                {
                    // !兼容老的TravelType2产品，按照TravelType1显示
                    var productDatePrice = p.GetPriceDate(selectedDepartureDate);
                    if (p.TravelType == TravelType.TravelType2 && productDatePrice.ProductBasicPrice != 0)
                    {
                        prodModel.TravelType = TravelType.TravelType1;
                        p.TravelType = TravelType.TravelType1;
                    }

                    var builder = new RoomFlightModelBuilder(p, selectedDepartureDate, year.Value, month.Value,
                        ProductService.GetTicketSegment, 
                        ProductService.GetHotelSegments,
                        ProductService.GetTickets,
                        ProductService.GetRoomClasses);
                    
                    ViewBag.RoomFlightModel = builder.CreateFrom(null);
                }// end else
            }
            else if (step.HasValue && step.Value == 1)
            {
                // query -> viewbag -> 日历组件入参
                ViewBag.Year = year;
                ViewBag.Month = month;
            }

            return View("Index", prodModel);
        }

        /// <summary>
        /// 旅游资讯
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public ActionResult Consultation(string productNo)
        {
            var p = ProductService.GetProduct(productNo);
            return View(p);
        }

        public void DownloadPDF(string productNo)
        {
            string fileNameWithoutExtention = productNo;
            string path = Server.MapPath(@"~/pdf/" + fileNameWithoutExtention + ".pdf");
            byte[] file;

            if (!System.IO.File.Exists(path))
            {
                System.Threading.Thread.Sleep(500);
                if (!System.IO.File.Exists(path))
                {
                    throw new FileNotFoundException("下载PDF失败", path);
                }
            }

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                file = new byte[fs.Length];
                fs.Read(file, 0, file.Length);
            }

            //Response给客户端下载
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=" + fileNameWithoutExtention + ".pdf");
            Response.ContentType = "binary/octet-stream";
            Response.BinaryWrite(file);
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="productNo"></param>
        /// <param name="tab"></param>
        /// <param name="pageIndex"></param>
        /// <param name="photoIndex"></param>
        /// <returns></returns>
        public ActionResult Index(string productNo,
            int? tab, int? pageIndex, int? photoIndex)
        {
            ProductModel prodModel;
            ViewBag.Tab = tab;

            var prod = ProductService.GetProduct(productNo);
            prodModel = new ProductModel(prod);

            DepartureCity? departureCity;
            if (prodModel != null)
            {
                departureCity = DepartureCity.Parse(prodModel.DepartureCity);
            }
            else
            {
                departureCity = CookieManager.DepartureCity;
            }

            BuildProductTopNavModelAndMeta(departureCity, prod);

            if (prod == null)
            {
                return RedirectToAction("NoResults", "Product");
            }

            if (tab == 1)
            {
                var pager = new Pager(pageIndex, 7);
                prodModel.ScheduleItemList = ProductService.GetScheduleItems(prodModel.ProductId, pager).ToList();

                ViewBag.PagerModel = new PagerModel
                {
                    PageIndex = pageIndex ?? 0,
                    PageCount = pager.PageCount,
                    GetUrl = (name, value, baseUrl) =>
                    {
                        string url = Url.Action("Index", new
                        {
                            productNo = productNo,
                            tab = tab,
                            pageIndex = value,
                        });
                        return url;
                    },
                };

                ViewBag.PhotoPagerModel = new PagerModel
                {
                    PageIndex = photoIndex ?? 0,
                    PageCount = prodModel.PhotoList.Count,
                    GetUrl = (name, value, baseUrl) =>
                    {
                        string url = string.Format("javascript:showPhoto({0});", value);
                        return url;
                    },
                };
            }
            else if (tab == 2)
            {
                prodModel.HotelList = ProductService.GetHotels(prodModel.ProductId).ToList();
            }
            else if (tab == 3)
            {
                ViewBag.Step = 1;
            }

            return View(prodModel);
        }

        /// <summary>
        /// 订购须知
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public ActionResult OrderNotes(string productNo)
        {
            ProductModel prodModel;
            var prod = ProductService.GetProduct(productNo);
            prodModel = new ProductModel(prod);
            return View(prodModel);
        }

        public ActionResult PDF(string productNo)
        {
            ProductModel prodModel;
            var prod = ProductService.GetProduct(productNo);
            if (prod == null)
            {
                return null;
            }
            prodModel = new ProductModel(prod);
            prodModel.HotelList = ProductService.GetHotels(prod.ProductId);
            prodModel.ScheduleItemList = ProductService.GetScheduleItems(prod.ProductId, null).ToList();
            return View(prodModel);
        }

        #region 转寄好友

        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public ActionResult Share(string productNo)
        {
            var prod = ProductService.GetProduct(productNo);

            if (prod == null)
            {
                return RedirectToAction("NoResults", "Product");
            }

            ShareModel model = new ShareModel
            {
                Title = string.Format("{0} | 鸿鹄逸游 HHtravel", prod.ProductName),
                ProductNo = prod.ProductNo,
                ProductName = prod.ProductName,
            };

            return View(model);
        }

        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Share(ShareModel model)
        {
            if (!CaptchaController.Validate(model.Captcha))
            {
                ModelState.AddModelError("Captcha", "验证码错误");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 生成邮件Html并发送
            string body = null;
            ShareEmailModel emailModel = new ShareEmailModel
            {
                Message = model.Message,
                ProductNo = model.ProductNo,
                ProductName = model.ProductName,
            };
            body = RenderRazorViewToString("ShareEmail", emailModel);
            Email email = new Email
            {
                Sender = model.YourEmail,
                Recipients = model.FriendEmail,
                Subject = model.Title,
                Body = body,
                EmailType = EmailType.Share,
            };
            EmailService.Add(email);

            return RedirectToActionPermanent("ShareSuccess", new { productNo = model.ProductNo });
        }

        // 邮件内容
        public ActionResult ShareEmail(string productNo, string message)
        {
            var p = ProductService.GetProduct(productNo);
            ShareEmailModel emailModel = new ShareEmailModel
            {
                Message = message,
                ProductNo = p.ProductNo,
                ProductName = p.ProductName,
            };
            return View(emailModel);
        }

        /// <summary>
        /// 分享成功
        /// </summary>
        /// <returns></returns>
        public ActionResult ShareSuccess()
        {
            return View();
        }

        #endregion 转寄好友
    }
}