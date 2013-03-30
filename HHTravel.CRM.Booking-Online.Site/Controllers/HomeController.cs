using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using HHTravel.CRM.Booking_Online.Site.Models;

using HHTravel.CRM.Booking_Online.Model;
using HHTravel.CRM.Booking_Online.Business.IApplicationService;
using HHTravel.CRM.Booking_Online.Business.ApplicationServiceImp;
using System.Text.RegularExpressions;

namespace HHTravel.CRM.Booking_Online.Site.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Default1/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexCn()
        {
            var now = DateTime.Now.Date;
            var endDate = now.AddMonths(1);

            ViewBag.SearchModel = new SearchBoxModel
            {
                DeparturePlaces = ProductService.GetAllDepartureCitys(),
                DestinationGroups = ProductService.GetAllDestinationGroups(),
                TravelTypes = ProductService.GetAllTravelTypes(),
                Interests = ProductService.GetAllInterests(),
                DepartureBeginDate = now,
                // todo: DepartureEndDate应为所有产品的最后可出发日期
                DepartureEndDate = endDate,
            };
            var model = new TopNavModel(false);

            return View();
        }
        // json
        public ActionResult GetDestinationRegions(int groupId)
        {
            var a = ProductService.GetAllDestinationGroups().Where(g => groupId == g.GroupId).Single();
            var b = from r in a.RegionList
                    select new { r.RegionId, r.Name };
            return Json(b, JsonRequestBehavior.AllowGet);
        }

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
        }

        /// <summary>
        /// 环游世界
        /// </summary>
        /// <returns></returns>
        public ActionResult AroundWorld()
        {
            ViewBag.TopNavModel = new TopNavModel(SiteConfig.TopImageAroundWorld,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel("目的地", Url.Action("Destination")),
                    new BreadcrumbModel("环游世界", null)
                });
            return View();
        }

        #region 顶部栏目

        /// <summary>
        /// 品牌理念
        /// </summary>
        /// <returns></returns>
        public ActionResult BrandConcept()
        {
            ViewBag.TopNavModel = new TopNavModel(SiteConfig.TopImageAboutUs,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel("品牌理念", null)
                });
            return View();
        }

        /// <summary>
        /// 目的地
        /// Note: 若修改Action名称，除View中的链接，
        /// 建议对TopNavModel查找引用，检查是否有对该Action的使用
        /// </summary>
        /// <returns></returns>
        public ActionResult Destination()
        {
            ViewBag.TopNavModel = new TopNavModel(SiteConfig.TopImageDestination,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel("目的地", null)
                });
            return View();
        }

        /// <summary>
        /// 出发月份
        /// </summary>
        /// <returns></returns>
        public ActionResult DepartMonth()
        {
            var list = ProductService.GetAllDepartureMonths();

            ViewBag.TopNavModel = new TopNavModel(SiteConfig.TopImageDeparture,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel("出发月份", null)
                });
            return View(list);
        }

        /// <summary>
        /// 旅行主题
        /// </summary>
        /// <returns></returns>
        public ActionResult Interest()
        {
            ViewBag.TopNavModel = new TopNavModel(SiteConfig.TopImageInterest,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel("旅行主题", null)
                });
            return View();
        }

        /// <summary>
        /// 热门品鉴
        /// </summary>
        /// <returns></returns>
        public ActionResult TopTasting()
        {
            // todo:
            ViewBag.TopNavModel = new TopNavModel(SiteConfig.TopImageUnique,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel("热门品鉴", null)
                });
            return View();
        }
        #endregion

        [HttpPost]
        public ActionResult Subscribe(string email)
        {
            Regex r = new Regex(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}");

            if (!r.IsMatch(email))
            {
                return Json(false);
            }

            NewsletterService.Subscribe(email);
            return Json(true);
        }

        #region 底部链接
        /// <summary>
        /// 隐私政策
        /// </summary>
        /// <returns></returns>
        public ActionResult PrivacyPolicy()
        {
            ViewBag.TopNavModel = new TopNavModel(SiteConfig.TopImagePrivacyPolicy,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel("隐私政策", null)
                });
            return View();
        }
        /// <summary>
        /// 首页的电子报订阅入口
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>

        /// <summary>
        /// 电子报订阅
        /// </summary>
        /// <returns></returns>
        public ActionResult Newsletter(string email)
        {
            ViewBag.TopNavModel = new TopNavModel(SiteConfig.TopImageNewsletter,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel("电子报订阅", null)
                });
            NewsletterModel model = new NewsletterModel();
            if (!string.IsNullOrEmpty(email))
            {
                model.Email = email;
            }

            return View(model);
        }
        /// <summary>
        /// 电子报订阅
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Newsletter(NewsletterModel model)
        {
            // todo: 如果成功跳转，则不需要设定TopNavModel
            ViewBag.TopNavModel = new TopNavModel(SiteConfig.TopImageNewsletter,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel("电子报订阅", null)
                });

            if (!CaptchaController.Validate(model.Captcha))
            {
                ModelState.AddModelError("Captcha", "验证码错误");
            }

            if (!string.Equals(model.Email, model.EmailAgain, StringComparison.InvariantCultureIgnoreCase))
            {
                ModelState.AddModelError("EmailAgain", "两次输入的E-mail不一致");
            }

            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            NewsletterService.Subscribe(model.Email);

            // 生成邮件Html并发送
            string body = null;
            body = RenderRazorViewToString("NewsletterEmail", model);
            Email email = new Email
            {
                Sender = "",
                Recipients = model.Email,
                Subject = "鸿鹄逸游电子报订阅成功信件",
                Body = body,
                EmailType = EmailType.NewsSub,
            };
            EmailService.Send(email);

            return Json(new { Success = true });
        }
        // 邮件内容
        public ActionResult NewsletterEmail(NewsletterModel model)
        {
            return View(model);
        }
        /// <summary>
        /// 联络我们
        /// </summary>
        /// <returns></returns>
        public ActionResult ContactUs()
        {
            ViewBag.TopNavModel = new TopNavModel(SiteConfig.TopImageContactUs,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel("联络我们", null)
                });
            return View();
        }
        #endregion
    }
}
