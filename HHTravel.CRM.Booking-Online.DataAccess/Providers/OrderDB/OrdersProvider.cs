using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.Entity;

namespace HHTravel.CRM.Booking_Online.DataAccess.Providers
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