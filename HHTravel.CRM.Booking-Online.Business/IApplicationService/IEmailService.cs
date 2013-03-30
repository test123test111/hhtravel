using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.Business.IApplicationService
{
    public interface IEmailService
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        void Send(Email email);
    }
}
