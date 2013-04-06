using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.Entity;

namespace HHTravel.CRM.Booking_Online.DataAccess.Providers
{
    public class CustomersProvider : CustomerDbProviderBase<Customer>
    {
        public CustomersProvider()
            : base()
        {
        }

        public CustomersProvider(CustomerDbEntities customerDbContext)
            : base(customerDbContext)
        {
        }
    }
}