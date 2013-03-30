﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.IRepository
{
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// 插入订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Order Insert(Order order);
    }
}