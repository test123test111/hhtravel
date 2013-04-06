using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HHTravel.CRM.Booking_Online.ApplicationService;
using HHTravel.CRM.Booking_Online.Infrastructure;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.Infrastructure.Web.Mvc;
using HHTravel.CRM.Booking_Online.Site.Models;

namespace HHTravel.CRM.Booking_Online.Site.Controllers
{
    public class HomeController : ControllerBase
    {
        [OutputCache(Duration = 600)]
        public PartialViewResult _LeftMenu()
        {
            var model = new LefuMenuModel
            {
                DestinationGroups = ProductService.GetAllDestinationGroups(),
                Interests = ProductService.GetAllInterests(),
                DepartureMonths = ProductService.GetAllDepartureMonths(),
            };

            return PartialView(model);
        }

        /// <summary>
        /// 环游世界
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600)]
        public ActionResult AroundWorld()
        {
            var model = new TopNavModel(SiteColumnService.GetTopImage(SiteColumn.环游世界栏目),
               new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.目的地, Url.Action("Destination")),
                    new BreadcrumbModel("环游世界", null)
                });

            ViewBag.TopNavModel = model;
            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.环游世界栏目);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }

            return View();
        }

        // json
        [OutputCache(Duration = 600)]
        public ActionResult GetDestinationRegions(int groupId)
        {
            var a = ProductService.GetAllDestinationGroups().Where(g => groupId == g.GroupId).Single();
            var b = from r in a.RegionList
                    select new { r.RegionId, r.Name };
            return Json(b, JsonRequestBehavior.AllowGet);
        }

        /*

        /// <summary>
        /// 全站首页
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600)]
        public ActionResult Index()
        {
            ViewBag.DepartureCitys = ProductService.GetAllDepartureCitys();
            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.全站首页);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }
            return View();
        }*/

        /// <summary>
        /// 主页
        /// </summary>
        /// <param name="cityCode">出发地城市编码</param>
        /// <returns></returns>
        //[OutputCache(Duration = 600, VaryByParam = "none", VaryByHeader = "Accept-Language")]
        public ActionResult Index()
        {
            if (GlobalConfig.DetectBrowserLanguage)
            {
                bool requestFromAddressBar = (this.Request.UrlReferrer == null);
                bool requestFromTB = requestFromAddressBar ? false : string.Equals(this.Request.UrlReferrer.Host, "tb.hhtravel.com", StringComparison.OrdinalIgnoreCase);
                bool requestFromSelf = requestFromAddressBar ? false : string.Equals(this.Request.UrlReferrer.Host, "www.hhtravel.com", StringComparison.OrdinalIgnoreCase);

                if (requestFromAddressBar || (!requestFromTB && !requestFromSelf))
                {
                    // http://stackoverflow.com/questions/6157485/content-language-and-accept-language
                    string lang = Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];
                    if (lang != null)
                    {
                        int indexTw = lang.IndexOf("zh-tw", StringComparison.OrdinalIgnoreCase);
                        int indexZh = lang.IndexOf("zh", StringComparison.OrdinalIgnoreCase);
                        int indexCn = lang.IndexOf("zh-cn", StringComparison.OrdinalIgnoreCase);
                        if (indexTw >= 0)
                        {
                            if (indexTw == 0)
                            {
                                return Redirect("http://tb.hhtravel.com");  // HARDCODE
                            }
                        }
                    }
                }
            }

            var now = DateTime.Now.Date;
            var endDate = now.AddMonths(1);

            var allCitys = ProductService.GetAllDepartureCitys();
            ViewBag.SearchModel = new SearchBoxModel
            {
                DepartureCitys = allCitys,
                DestinationGroups = ProductService.GetAllDestinationGroups(),
                TravelTypes = ProductService.GetAllTravelTypes(),
                Interests = ProductService.GetAllInterests(),
                DepartureBeginDate = now,
                DepartureEndDate = endDate,// todo: DepartureEndDate应为所有产品的最后可出发日期
                DaysSections = SelectListFactory.Create<DaysSection, int>(true),
            };
            ViewBag.TopNavModel = new TopNavModel(false);
            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.简体首页);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }

            return View();
        }

        /*
        public PartialViewResult _TopNav()
        {
            var cxt = this.ControllerContext;
            var parentActionRouteValues = cxt.ParentActionViewContext.RouteData.Values;
            System.Diagnostics.Debug.Assert(cxt.IsChildAction);

            string controller = parentActionRouteValues["Controller"].ToString();
            string action = parentActionRouteValues["Action"].ToString();

            //string id = parentActionRouteValues["id"].ToString();

            // todo: 设置默认model用于容错
            return PartialView();
        }*/

        #region 顶部栏目

        /// <summary>
        /// 品牌理念
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600)]
        public ActionResult BrandConcept()
        {
            ViewBag.TopNavModel = new TopNavModel(SiteColumnService.GetTopImage(SiteColumn.品牌理念),
                new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.品牌理念, null)
                });
            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.品牌理念);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }
            return View();
        }

        /// <summary>
        /// 出发月份
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600)]
        public ActionResult DepartMonth()
        {
            var list = ProductService.GetAllDepartureMonths();

            ViewBag.TopNavModel = new TopNavModel(SiteColumnService.GetTopImage(SiteColumn.出发月份),
                CookieManager.DepartureCity,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.出发月份, null)
                });
            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.出发月份);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }
            return View(list);
        }

        /// <summary>
        /// 目的地
        /// Note: 若修改Action名称，除View中的链接，
        /// 建议对TopNavModel查找引用，检查是否有对该Action的使用
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600)]
        public ActionResult Destination()
        {
            ViewBag.TopNavModel = new TopNavModel(SiteColumnService.GetTopImage(SiteColumn.目的地),
                CookieManager.DepartureCity,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.目的地, null)
                });
            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.目的地);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }
            return View();
        }

        /// <summary>
        /// 旅行主题
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600)]
        public ActionResult Interest()
        {
            ViewBag.TopNavModel = new TopNavModel(SiteColumnService.GetTopImage(SiteColumn.旅行主题),
                CookieManager.DepartureCity,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.旅行主题, null)
                });
            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.旅行主题);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }
            return View();
        }

        /// <summary>
        /// 热门品鉴
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600)]
        public ActionResult TopTasting()
        {
            // todo:
            ViewBag.TopNavModel = new TopNavModel(SiteColumnService.GetTopImage(SiteColumn.热门品鉴),
                new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.热门品鉴, null)
                });
            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.热门品鉴);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }
            return View();
        }

        #endregion 顶部栏目

        #region 底部链接

        /// <summary>
        /// 联络我们
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600)]
        public ActionResult ContactUs()
        {
            ViewBag.TopNavModel = new TopNavModel(SiteColumnService.GetTopImage(SiteColumn.联络我们),
                new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.联络我们, null)
                });
            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.联络我们);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }
            return View();
        }

        [OutputCache(Duration = 600)]
        public ActionResult CooperationArea()
        {
            ViewBag.TopNavModel = new TopNavModel(SiteColumnService.GetTopImage(SiteColumn.合作专区),
               new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.合作专区, null)
                });
            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.合作专区);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }
            return View();
        }

        [OutputCache(Duration = 600)]
        public ActionResult FriendLinks()
        {
            ViewBag.TopNavModel = new TopNavModel(SiteColumnService.GetTopImage(SiteColumn.友情链接),
               new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.友情链接, null)
                });
            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.友情链接);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }
            return View();
        }

        /// <summary>
        /// 隐私政策
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600)]
        public ActionResult PrivacyPolicy()
        {
            ViewBag.TopNavModel = new TopNavModel(SiteColumnService.GetTopImage(SiteColumn.隐私政策),
                new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.隐私政策, null)
                });
            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.隐私政策);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }
            return View();
        }

        /// <summary>
        /// 站点地图
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600)]
        public ActionResult Sitemap()
        {
            var model = new SitemapModel
            {
                DepartureCitys = ProductService.GetAllDepartureCitys(),
                DestinationGroups = ProductService.GetAllDestinationGroups(),
                Interests = ProductService.GetAllInterests(),
                DepartureMonths = ProductService.GetAllDepartureMonths(),
            };

            var topNavModel = new TopNavModel(null,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.网站地图栏目, null)
                });
            topNavModel.HideTopImage = true;
            ViewBag.TopNavModel = topNavModel;

            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.网站地图栏目);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }

            return View(model);
        }

        #endregion 底部链接

        /// <summary>
        /// 网站地图xml版
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600)]
        public ActionResult SitemapXml()
        {
            var model = new SitemapXmlModel
            {
                Dns = string.Format("{0}://{1}", HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Host),
                ProductNos = ProductService.GetAllProductNos(),
                DepartureCitys = ProductService.GetAllDepartureCitys(),
                DestinationGroups = ProductService.GetAllDestinationGroups(),
                Interests = ProductService.GetAllInterests(),
                DepartureMonths = ProductService.GetAllDepartureMonths(),
            };
            return View(model);
        }
    }
}