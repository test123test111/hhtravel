using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HHTravel.Base.Common.Framework.Logging;

namespace HHTravel.CRM.Booking_Online.Site.Filter
{
    public class ExceptionFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            #region 记录日志
            SysLog.WriteTrace(filterContext.Exception.ToString());
            #endregion

            base.OnException(filterContext);
        }
    }
}