using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HHTravel.CRM.Booking_Online.Business;
using HHTravel.CRM.Booking_Online.Business.IApplicationService;
using HHTravel.CRM.Booking_Online.Business.ApplicationServiceImp;
using HHTravel.CRM.Booking_Online.Model;
using System.IO;

namespace HHTravel.CRM.Booking_Online.Site.Controllers
{
    public abstract class BaseController : Controller
    {
        private IProductService _productService;

        protected IProductService ProductService
        {
            get
            {
                if (_productService == null)
                {
                    _productService = new ProductServiceImp();
                }
                return _productService;
            }
        }
        private IOrderService _orderService;

        protected IOrderService OrderService
        {
            get
            {
                if (_orderService == null)
                {
                    _orderService = new OrderServiceImp();
                }
                return _orderService;
            }
        }
        private INewsletterService _newsletterService;

        protected INewsletterService NewsletterService
        {
            get
            {
                if (_newsletterService == null)
                {
                    _newsletterService = new NewsletterServiceImp();
                }
                return _newsletterService;
            }
        }

        private IEmailService _emailService;

        protected IEmailService EmailService
        {
            get
            {
                if (_emailService == null)
                {
                    _emailService = new EmailServiceImp();
                }
                return _emailService;
            }
        }

        private ISiteConfigService _siteConfigService;

        protected ISiteConfigService SiteConfigService
        {
            get
            {
                if (_siteConfigService == null)
                {
                    _siteConfigService = new SiteConfigServiceImp();
                }
                return _siteConfigService;
            }
        }
        
        private SiteConfig _siteConfig;
        /// <summary>
        /// 站点配置
        /// </summary>
        protected SiteConfig SiteConfig
        {
            get
            {
                if (_siteConfig == null)
                {
                    _siteConfig = this.SiteConfigService.GetSiteConfig();
                }
                return _siteConfig;
            }
        }

        // http://stackoverflow.com/questions/483091/render-a-view-as-a-string
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
