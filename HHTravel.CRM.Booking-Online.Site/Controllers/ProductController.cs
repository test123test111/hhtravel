using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using HHTravel.CRM.Booking_Online.Model;
using HHTravel.CRM.Booking_Online.Site.Models;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace HHTravel.CRM.Booking_Online.Site.Controllers
{
    public class ProductController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 组合搜索
        /// </summary>
        /// <param name="departureCity"></param>
        /// <param name="destinationGroupId"></param>
        /// <param name="destinationRegionId"></param>
        /// <param name="travelTypeId"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="interestid"></param>
        /// <param name="travelType"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult Search(string departureCity, int? destinationGroupId, int? destinationRegionId,
            int? travelTypeId, DateTime? beginDate, DateTime? endDate,
            int? interestid, int? travelType,
            int? sort, bool? descending, int? pageIndex)
        {
            string title = "";
            // 全部出发地
            if (departureCity == "null")
            {
                departureCity = null;
            }

            DestinationGroup group;
            DestinationRegion region;
            ViewBag.TopNavModel = CreateDestionationTopNavModel(destinationGroupId, destinationRegionId, ref title,
                out group, out region);
            // HARDCODE 环游世界
            if (destinationGroupId.HasValue && destinationGroupId.Value == 43)
            {
                return RedirectToAction("AroundWorld", "Home");
            }

            Pager pager = new Pager(sort, descending, pageIndex);
            var plist = ProductService.Search(departureCity, destinationGroupId, destinationRegionId, travelTypeId, beginDate, endDate, interestid,
                            pager);

            if (travelType.HasValue)
            {
                plist = plist.Where(p => travelType.Value == (int)p.TravelType);
            }

            ProductListModel model = new ProductListModel
            {
                ProductList = plist,
                TravelType = travelType ?? travelTypeId,
                Sort = sort,
                Descending = descending,
                PagerModel = new PagerModel
                {
                    PageIndex = pageIndex ?? 0,
                    PageCount = pager.PageCount,
                    BaseUrl = Url.Action("Search", new
                    {
                        departureCity = departureCity,
                        destinationGroupId = destinationGroupId,
                        destinationRegionId = destinationRegionId,
                        travelTypeId = travelTypeId,
                        beginDate = beginDate,
                        endDate = endDate,
                        interestid = interestid,
                        travelType = travelType,
                        pageIndex = pageIndex,
                        sort = sort,
                        descending = descending,
                    }),
                    GetUrl = GetPagerUrl4List,
                },
            };

            return View("List", model);
        }
        /// <summary>
        /// 按分类搜索
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="propertyValue"></param>
        /// <param name="childValue"></param>
        /// <param name="travelType"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult FindBy(int? propertyType, int? propertyValue, int? childValue, int? travelType,
            int? sort, bool? descending, int? pageIndex)
        {
            IEnumerable<Product> plist = new List<Product>();
            TopNavModel topNavModel = null;
            string title = "";
            Pager pager = new Pager(sort, descending, pageIndex);

            if (!propertyType.HasValue)
            {
                topNavModel = CreateDefaultTopNavModel(null);
            }
            else
            {
                if (propertyType == 1)  // 按目的地
                {
                    DestinationGroup group;
                    DestinationRegion region;
                    int? groupId = propertyValue;
                    int? regionId = childValue;
                    topNavModel = CreateDestionationTopNavModel(groupId, regionId, ref title, out group, out region);

                    if (group != null)
                    {
                        regionId = (region != null) ? region.RegionId : (int?)null;
                        plist = ProductService.Search(null, group.GroupId, regionId, travelType, null, null, null, pager);
                    }
                }
                else if (propertyType == 2) // 按旅行主题
                {
                    Interest interest;
                    int? interestId = propertyValue;
                    topNavModel = CreateInterestTopNavModel(interestId, ref title, out interest);

                    if (interest != null)
                    {
                        plist = ProductService.Search(null, null, null, travelType, null, null, interest.InterestId, pager);
                    }
                }
                else if (propertyType == 3) // 按出发月份
                {
                    string inputDateString = string.Format("{0}-{1}-{2}", propertyValue, (childValue.HasValue) ? childValue.Value : 1, 1);
                    DateTime beginDate;
                    if (!DateTime.TryParse(inputDateString, out beginDate))
                    {
                        topNavModel = CreateDefaultTopNavModel("出发月份");
                    }
                    else
                    {
                        DateTime endDate = beginDate.AddMonths(1);

                        title = "出发月份";
                        topNavModel = new TopNavModel(
                                SiteConfig.TopImageDeparture,
                                new List<BreadcrumbModel> { 
                            new BreadcrumbModel("出发月份", Url.Action("DepartMonth", "Home")),
                            new BreadcrumbModel(beginDate.ToString("yyyy-MM月"), null),
                        });
                        plist = ProductService.Search(null, null, null, travelType, beginDate, endDate, null, pager);
                    }
                }
                else
                {
                    topNavModel = CreateDefaultTopNavModel(null);
                }
            }

            #region Build View Model
            ProductListModel model = new ProductListModel
            {
                ProductList = plist,
                TravelType = travelType,
                Sort = sort,
                Descending = pager.Descending,
                PagerModel = new PagerModel
                {
                    PageIndex = pageIndex ?? 0,
                    PageCount = pager.PageCount,
                    BaseUrl = Url.Action("FindBy", new
                    {
                        propertyType = propertyType,
                        propertyValue = propertyValue,
                        childValue = childValue,
                        travelType = string.Format("{0}", travelType),
                        pageIndex = string.Format("{0}", pageIndex),
                        sort = string.Format("{0}", sort),
                        descending = descending,
                    }),
                    GetUrl = GetPagerUrl4List,
                },
            };

            ViewBag.TopNavModel = topNavModel;
            ViewBag.Title = title;
            #endregion

            return View("List", model);
        }


        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="productNo"></param>
        /// <param name="tab"></param>
        /// <param name="pageIndex"></param>
        /// <param name="photoIndex"></param>
        /// <returns></returns>
        public ActionResult Details(string productNo, int? tab, int? pageIndex, int? photoIndex)
        {
            TopNavModel topNavModel;
            var p = ProductService.GetProduct(productNo);

            if (p == null)
            {
                topNavModel = CreateDefaultTopNavModel(null);
                ViewBag.TopNavModel = topNavModel;
                return View("List", new ProductListModel { ProductList = new List<Product>() });
            }

            var group = ProductService.GetDestinationGroups(p.ProductId).First();
            var region = ProductService.GetDestinationRegions(p.ProductId).First();
            var regionTopImage = (region != null && region.TopImage != null) ? region.TopImage : group.TopImage;

            topNavModel = new TopNavModel(
                        p.TopImage ?? regionTopImage,
                        new List<BreadcrumbModel> { 
                            new BreadcrumbModel(group.Name, Url.Action("FindBy", "Product", new {propertyType=1, propertyValue=group.GroupId})),
                            new BreadcrumbModel(region.Name, Url.Action("FindBy", "Product", new {propertyType=1, propertyValue=group.GroupId, childValue = region.RegionId})),
                            new BreadcrumbModel(p.ProductName, null),
                        });


            ViewBag.Tab = tab;
            ViewBag.TopNavModel = topNavModel;

            if (tab == 1)
            {
                Pager pager = new Pager(pageIndex);
                // todo: ScheduleItemList单独作为ViewModel的一部分
                p.ScheduleItemList = ProductService.GetScheduleItems(p.ProductId, pager).ToList();

                ViewBag.PagerModel = new PagerModel
                {
                    PageIndex = pageIndex ?? 0,
                    PageCount = pager.PageCount,
                    GetUrl = (name, value, baseUrl) =>
                    {
                        string url = Url.Action("Details", new
                        {
                            productNo = productNo,
                            tab = tab,
                            pageIndex = value,
                        });
                        return url;
                    },
                };
                ViewBag.PhotoList = p.PhotoList;
                ViewBag.PhotoPagerModel = new PagerModel
                {
                    PageIndex = photoIndex ?? 0,
                    PageCount = p.PhotoList.Count,
                    GetUrl = (name, value, baseUrl) =>
                    {
                        string url = string.Format("javascript:showPhoto({0});", value);
                        return url;
                    },
                };
            }
            else if (tab == 3)
            {
                ViewBag.Step = 1;
            }

            return View(p);
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
        public ActionResult CalendarRoomFlight(string productNo, string date, int year, int month, int? step)
        {
            var p = ProductService.GetProduct(productNo);

            var group = ProductService.GetDestinationGroups(p.ProductId).First();
            var region = ProductService.GetDestinationRegions(p.ProductId).First();
            var regionTopImage = (region != null && region.TopImage != null) ? region.TopImage : group.TopImage;

            ViewBag.TopNavModel = new TopNavModel(
                        p.TopImage ?? regionTopImage,
                        new List<BreadcrumbModel> { 
                            new BreadcrumbModel(group.Name, Url.Action("FindBy", "Product", new {propertyType=1, propertyValue=group.GroupId})),
                            new BreadcrumbModel(region.Name, Url.Action("FindBy", "Product", new {propertyType=1, propertyValue=group.GroupId, childValue = region.RegionId})),
                            new BreadcrumbModel(p.ProductName, null),
                        });
            ViewBag.Tab = 3;
            ViewBag.Step = step ?? 2;   // 默认值是展示第二步

            DateTime selectedDepartureDate;
            bool pass = DateTime.TryParseExact(date, "yyyyMMdd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out selectedDepartureDate);

            if (!step.HasValue || step.Value == 2)
            {
                // 如果第二步的入参日期无效
                if (!pass)
                {
                    ViewBag.Step = 1;
                }
                else
                {
                    List<Ticket> ticketList = ProductService.GetTickets(p.ProductId, selectedDepartureDate, true).ToList();
                    List<RoomClass> roomClassList = ProductService.GetRoomClasses(p.ProductId, selectedDepartureDate, true).ToList();

                    var model = new RoomFilghtModel
                    {
                        ProductNo = p.ProductNo,
                        Year = year,
                        Month = month,
                        DepartureDate = selectedDepartureDate,
                        ReturnDate = selectedDepartureDate.AddDays(p.Days - 1), // 计算预定的回程日期
                        MaxPersonNum = p.MaxPersonNum,
                        RoomClassList = roomClassList,
                        TicketList = ticketList,

                        NightCount = p.MinLodgingDays ?? 0,
                    };
                    ViewBag.RoomFilghtModel = model;
                }// end else
            }
            else if (step.HasValue && step.Value == 1)
            {
                // query -> viewbag -> 日历组件入参
                ViewBag.Year = year;
                ViewBag.Month = month;
            }

            return View("Details", p);
        }

        #region 转寄好友
        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public ActionResult Share(string productNo)
        {
            var p = ProductService.GetProduct(productNo);

            ShareModel model = new ShareModel
            {
                Title = string.Format("{0} | 鸿鹄逸游 HHtravel", p.ProductName),
                ProductNo = p.ProductNo,
                ProductName = p.ProductName,
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
            EmailService.Send(email);

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
        #endregion

        // just for test
        public ActionResult PDF(string productNo)
        {
            var p = ProductService.GetProduct(productNo);
            return View(p);
        }

        public void DownloadPDF(string productNo)
        {
            string fileNameWithoutExtention = Guid.NewGuid().ToString();
            //執行wkhtmltopdf.exe
            string exePath = System.Web.HttpContext.Current.Server.MapPath(@"~/html2pdf/wkhtmltopdf.exe");
            string url = Url.Action("PDF", "Product", new { productNo = productNo }, "http");
            //"www.hhtravel.com/hTravel/event/2012_christmas/christmas_cn.html?departArea=SHA";
            string path = Server.MapPath(@"~/pdf/" + fileNameWithoutExtention + ".pdf");
            if (!System.IO.File.Exists(exePath))
            {
                throw new Exception("下载PDF失败");
            }

            Process p = new Process();
            p.StartInfo.FileName = exePath;
            p.StartInfo.Arguments = " \"" + url + "\" " + path;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            System.Threading.Thread.Sleep(500);
            byte[] file;

            if (!System.IO.File.Exists(path))
            {
                throw new Exception("下载PDF失败");
            }

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                file = new byte[fs.Length];
                fs.Read(file, 0, file.Length);
            }
            //Response给客户端下载
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=" + fileNameWithoutExtention + ".pdf");
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(file);
        }

        /// <summary>
        /// 订购须知
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public ActionResult OrderNotes(string productNo)
        {
            var p = ProductService.GetProduct(productNo);
            return View(p);
        }

        /// <summary>
        /// 旅游咨询
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public ActionResult Consultation(string productNo)
        {
            var p = ProductService.GetProduct(productNo);
            return View(p);
        }

        #region helper
        private TopNavModel CreateDefaultTopNavModel(string breadcrumbText)
        {
            TopNavModel topNavModel;
            switch (breadcrumbText)
            {
                case "目的地":
                case "旅行主题":
                case "出发月份":
                    topNavModel = new TopNavModel(
                        SiteConfig.TopImageDeparture,
                        new List<BreadcrumbModel>{
                            new BreadcrumbModel(breadcrumbText, null)
                        });
                    break;
                default:
                    topNavModel = new TopNavModel(
                        SiteConfig.TopImageDeparture,
                        new List<BreadcrumbModel>
                        {
                            //new BreadcrumbModel(propertyType, null)
                        });
                    break;
            }
            return topNavModel;
        }

        private TopNavModel CreateInterestTopNavModel(int? interestId, ref string title, out Interest interest)
        {
            interest = null;
            TopNavModel topNavModel;

            if (!interestId.HasValue)
            {
                topNavModel = CreateDefaultTopNavModel("旅行主题");
                return topNavModel;
            }

            interest = ProductService.GetAllInterests().SingleOrDefault(i => i.InterestId == interestId);

            if (interest == null)
            {
                topNavModel = CreateDefaultTopNavModel("旅行主题");
                return topNavModel;
            }

            title = interest.Name;
            topNavModel = new TopNavModel(
                    interest.TopImage,
                    new List<BreadcrumbModel> { 
                            new BreadcrumbModel("旅行主题", Url.Action("Interest", "Home")),
                            new BreadcrumbModel(interest.Name, null),
                        });
            return topNavModel;
        }

        private TopNavModel CreateDestionationTopNavModel(int? groupId, int? regionId, ref string title,
            out DestinationGroup group, out DestinationRegion region)
        {
            group = null;
            region = null;
            TopNavModel topNavModel;

            if (!groupId.HasValue)
            {
                topNavModel = CreateDefaultTopNavModel("目的地");
                return topNavModel;
            }

            // 如果有多个，则取第一个目的地分组
            group = ProductService.GetAllDestinationGroups().FirstOrDefault(g => g.GroupId == groupId);

            if (group == null)
            {
                topNavModel = CreateDefaultTopNavModel("目的地");
                return topNavModel;
            }

            if (!regionId.HasValue)
            {
                //title = group.Name;
                topNavModel = new TopNavModel(
                    group.TopImage,
                    new List<BreadcrumbModel>{
                            new BreadcrumbModel("目的地", Url.Action("Destination", "Home")),
                            new BreadcrumbModel(group.Name, null),
                        });
            }
            else
            {
                // 如果有多个，则取第一个目的地区域
                region = group.RegionList.First(r => r.RegionId == regionId.Value);
                //title = region.Name;
                topNavModel = new TopNavModel(
                    region.TopImage ?? group.TopImage,
                    new List<BreadcrumbModel>{
                            new BreadcrumbModel("目的地", Url.Action("Destination", "Home")),
                            new BreadcrumbModel(group.Name, Url.Action("FindBy", new { propertyType = 1, propertyValue = group.GroupId })),
                            new BreadcrumbModel(region.Name, null),
                        });
            }

            return topNavModel;
        }
        /// <summary>
        /// 生成Pager的url
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        private string GetPagerUrl4List(string name, int? value, string baseUrl)
        {
            string newUrl;

            string patternFormat = @"{0}=(true|false|[\d])?";
            string pattern = string.Format(patternFormat, name);
            var r = new System.Text.RegularExpressions.Regex(pattern);

            string pair = string.Format("{0}={1}", name, value);
            if (r.IsMatch(baseUrl))
            {
                newUrl = r.Replace(baseUrl, pair);
            }
            else
            {
                newUrl = baseUrl.Contains("?") ? baseUrl + "&" + pair : baseUrl + "?" + pair;
            }

            patternFormat = @"{0}=(true|false|[\d])?&?";
            if (string.Equals(name, "travelType", StringComparison.OrdinalIgnoreCase))
            {
                pattern = string.Format(patternFormat, "pageIndex");
                r = new Regex(pattern, RegexOptions.IgnoreCase);
                newUrl = r.Replace(newUrl, "");
            }

            pattern = string.Format(patternFormat, "descending");
            r = new System.Text.RegularExpressions.Regex(pattern, RegexOptions.IgnoreCase);
            newUrl = r.Replace(newUrl, "");

            return newUrl;
        }
        #endregion
    }
}
