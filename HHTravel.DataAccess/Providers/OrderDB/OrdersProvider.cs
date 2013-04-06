using HHTravel.DataAccess.DbContexts;
using HHTravel.Entity;

namespace HHTravel.DataAccess.Providers
{
    public class OrdersProvider : OrderDbProviderBase<Orders>
    {
        public OrdersProvider()
            : base()
        {
        }

        public OrdersProvider(OrderDbEntities orderDbContext)
            : base(orderDbContext)
        {
        }
    }
}