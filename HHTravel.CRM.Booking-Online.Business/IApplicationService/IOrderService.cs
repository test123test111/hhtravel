using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.Business.IApplicationService
{
    [ServiceContract]
    public interface IOrderService
    {
        /// <summary>
        /// 插入订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Order Insert(Order order);
        /// <summary>
        /// 插入订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="additionalOrderItems"></param>
        /// <returns></returns>
        Order Insert(Order order, IList<OrderItem> additionalOrderItems);
    }
}
