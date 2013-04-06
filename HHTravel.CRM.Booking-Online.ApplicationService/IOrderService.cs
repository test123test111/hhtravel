using System.ServiceModel;
using HHTravel.CRM.Booking_Online.DomainModel;

namespace HHTravel.CRM.Booking_Online.ApplicationService
{
    [ServiceContract]
    public interface IOrderService
    {
        /// <summary>
        /// 插入订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Order Add(Order order);
    }
}