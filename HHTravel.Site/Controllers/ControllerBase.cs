using System.IO;
using System.Web.Mvc;
using HHTravel.ApplicationService;
using HHTravel.ApplicationService.ApplicationServiceImp;
using HHTravel.Infrastructure.Crosscutting;
using HHTravel.DomainModel;

namespace HHTravel.Site.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private IEmailService _emailService;
        private IHtmlMetaService _htmlMetaService;
        private IImageService _imageService;
        private INewsletterService _newsletterService;
        private IOrderService _orderService;
        private IProductService _productService;
        private ISiteColumnService _siteColumnService;

        public IEmailService EmailService
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

        public IHtmlMetaService HtmlMetaService
        {
            get
            {
                if (_htmlMetaService == null)
                {
                    _htmlMetaService = new HtmlMetaServiceImp();
                }
                return _htmlMetaService;
            }
        }

        public IImageService ImageService
        {
            get
            {
                if (_imageService == null)
                {
                    _imageService = new ImageServiceImp();
                }
                return _imageService;
            }
        }

        public ISiteColumnService SiteColumnService
        {
            get
            {
                if (_siteColumnService == null)
                {
                    _siteColumnService = new SiteColumnServiceImp();
                }
                return _siteColumnService;
            }
        }

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

        protected void SendOrderEmail(string emailTemplateName, object model, Order order)
        {
            string body = RenderRazorViewToString(emailTemplateName, model);

            Email email = new Email
            {
                Sender = null,
                Recipients = order.Email,
                Subject = string.Format("鸿鹄产品【{0}】订单提交邮件", order.Product.ProductName),
                Body = body,
                EmailType = EmailType.Order,
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
            };
            EmailService.Add(email);
        }

        // http://stackoverflow.com/questions/483091/render-a-view-as-a-string
        protected string RenderRazorViewToString(string viewName, object model)
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