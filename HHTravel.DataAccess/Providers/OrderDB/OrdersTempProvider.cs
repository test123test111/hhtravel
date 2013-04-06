using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Entity;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;

namespace HHTravel.CRM.Booking_Online.DataAccess.Providers.OrderDB
{
    public class OrdersTempProvider : OrderDbProviderBase<Orders_Temp>
    {
        public OrdersTempProvider()
            : base()
        {
        }

        public OrdersTempProvider(OrderDbEntities orderDbContext)
            : base(orderDbContext)
        {
        }
    }
}
