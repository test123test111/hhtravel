using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using HHTravel.DomainModel;
using HHTravel.Infrastructure.Crosscutting;
using HHTravel.Site.Filter;
using HHTravel.Site.Models;

namespace HHTravel.Site.Controllers
{
    public class NewsletterController : ControllerBase
    {
        /// <summary>
        /// 电子报订阅
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string email)
        {
            ViewBag.TopNavModel = new TopNavModel(SiteColumnService.GetTopImage(SiteColumn.电子报订阅),
                new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.电子报订阅, null)
                });
            NewsletterModel model = new NewsletterModel();
            if (!string.IsNullOrEmpty(email))
            {
                model.Email = email;
            }

            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.电子报订阅);
            if (meta != null)
            {
                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }

            return View(model);
        }

        /// <summary>
        /// 电子报订阅
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, NoCache]
        public ActionResult Index(NewsletterModel model)
        {
            ViewBag.TopNavModel = new TopNavModel(SiteColumnService.GetTopImage(SiteColumn.电子报订阅),
                new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.电子报订阅, null)
                });

            if (!CaptchaController.Validate(model.Captcha))
            {
                ModelState.AddModelError("Captcha", "验证码错误");
            }

            if (!string.Equals(model.Email, model.EmailAgain, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("EmailAgain", "两次输入的E-mail不一致");
            }

            Subscription sub = NewsletterService.Find(model.Email);
            if (!model.IsSubscription)
            {
                if (sub == null || !sub.IsValid)
                {
                    ModelState.AddModelError("Email", "您尚未订阅电子报");
                }
            }
            else
            {
                if (sub != null && sub.IsValid)
                {
                    ModelState.AddModelError("Email", "您已经订阅电子报");
                }
            }

            if (!ModelState.IsValid)
            {
                return PartialView("_Form", model);
            }

            if (model.IsSubscription)
            {
                NewsletterService.Subscribe(model.Email);
            }
            else
            {
                NewsletterService.Unsubscribe(model.Email);
            }

            SendNotifyEmail(model);

            return Json(new { Success = true, IsSubscription = model.IsSubscription });
        }

        /// <summary>
        /// 首页的电子报订阅入口
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost, NoCache]
        public ActionResult Subscribe(string email)
        {
            Regex r = new Regex(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}");

            if (!r.IsMatch(email))
            {
                return Json(new { IsSuccess = false, IsFormatError = true });
            }

            Subscription sub = NewsletterService.Find(email);
            if (sub != null && sub.IsValid)
            {
                return Json(new { IsSuccess = false, AlreadySubscribed = true });
            }

            NewsletterService.Subscribe(email);

            // 生成邮件Html并发送
            NewsletterModel model = new NewsletterModel
            {
                IsSubscription = true,
                Email = email,
            };
            SendNotifyEmail(model);

            return Json(new { IsSuccess = true });
        }

        private void SendNotifyEmail(NewsletterModel model)
        {
            // 生成邮件Html并发送
            string body = null;
            string subject = null;
            if (model.IsSubscription)
            {
                subject = "鸿鹄逸游电子报订阅成功信件";
                body = RenderRazorViewToString("SubscribeSuccessEmail", model);
            }
            else
            {
                subject = "鸿鹄逸游电子报取消订阅成功信件";
                body = RenderRazorViewToString("UnsubscribeSuccessEmail", model);
            }

            Email email = new Email
            {
                Sender = "",
                Recipients = model.Email,
                Subject = subject,
                Body = body,
                EmailType = EmailType.NewsSub,
            };

            EmailService.Add(email);
        }
    }
}