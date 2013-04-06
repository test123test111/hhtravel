using HHTravel.DataAccess.DbContexts;
using HHTravel.Entity;

namespace HHTravel.DataAccess.Providers
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