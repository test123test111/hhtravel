using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.Entity;
using HHTravel.DataAccess.DbContexts;

namespace HHTravel.DataAccess.Providers.OrderDB
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
