using System.Web.Mvc;

namespace HHTravel.CRM.Booking_Online.Site.Filter
{
    public class ExceptionFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            #region 记录日志

            var remark = filterContext.HttpContext != null ? filterContext.HttpContext.Request.RawUrl : "";

            //SysLog.WriteException("Global Exceptioin", filterContext.Exception, remark);

            #endregion 记录日志

            base.OnException(filterContext);
        }
    }
}