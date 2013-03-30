using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.Entity;
using Microsoft.Practices.Unity;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;

namespace HHTravel.CRM.Booking_Online.DataAccess.TableWorkers
{
    internal class PropertyTableWorker : ProductDbTableWorkerBase<Property>
    {
        public PropertyTableWorker()
            : base(DbContextFactory.Create<ProductDbEntities>())
        {
            
        }

        public PropertyTableWorker(ProductDbEntities productDbContext)
            : base(productDbContext)
        {

        }

        public IQueryable<Property> FindBy(string parentPath)
        {
            var a = from p in this.All()
                    where p.ParentPropertyPath == parentPath
                    select p;
            return a;
        }

        public IQueryable<Property> FindBy(PropertyPath parentPropertyPath)
        {
            string parentPath = ((int)parentPropertyPath).ToString();
            var a = this.FindBy(parentPath);
            return a;
        }

        public override Property Find(int id)
        {
            var a = from p in this.All()
                    where p.PropertyId == id
                    select p;
            var b = a.SingleOrDefault();
            return b;
        }
    }
}
