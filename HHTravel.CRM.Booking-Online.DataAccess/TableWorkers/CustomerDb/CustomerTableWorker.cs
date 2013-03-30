using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Entity;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;

namespace HHTravel.CRM.Booking_Online.DataAccess.TableWorkers
{
    internal class CustomerTableWorker : CustomerDbTableWorkerBase<Customer>
    {
        public CustomerTableWorker() : base()
        {
        }

        public CustomerTableWorker(CustomerDbEntities customerDbContext)
            : base(customerDbContext)
        {
        }
    }
}
