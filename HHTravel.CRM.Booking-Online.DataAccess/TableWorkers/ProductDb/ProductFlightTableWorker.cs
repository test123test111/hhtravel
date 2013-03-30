using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Entity;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;

namespace HHTravel.CRM.Booking_Online.DataAccess.TableWorkers.ProductDb
{
    internal class ProductFlightTableWorker : ProductDbTableWorkerBase<Product_Flight>
    {
        public ProductFlightTableWorker()
            : base(DbContextFactory.Create<ProductDbEntities>())
        {

        }

        public ProductFlightTableWorker(ProductDbEntities productDbContext)
            : base(productDbContext)
        {
        }

        public override Product_Flight Find(int id)
        {
            throw new NotImplementedException();
        }
    }
}
