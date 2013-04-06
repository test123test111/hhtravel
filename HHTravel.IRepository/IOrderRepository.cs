using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.DomainModel;

namespace HHTravel.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// 插入订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Order Add(Order order);
    }
}