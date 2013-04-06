using System.ServiceModel;
using HHTravel.DomainModel;

namespace HHTravel.ApplicationService
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