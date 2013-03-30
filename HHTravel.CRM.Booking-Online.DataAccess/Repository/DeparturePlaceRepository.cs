using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Model;
using Microsoft.Practices.Unity;
using System.Data.Entity;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.TableWorkers;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;

namespace HHTravel.CRM.Booking_Online.DataAccess.Repository
{
    public class DeparturePlaceRepository : RepositoryBase<DeparturePlace>, IDeparturePlaceRepository
    {
        [InjectionConstructor]
        public DeparturePlaceRepository()
        {
            this.ProductDbContext = DbContextFactory.Create<ProductDbEntities>();
        }

        internal DeparturePlaceRepository(ProductDbEntities productDbContext)
        {
            this.ProductDbContext = productDbContext;
        }

        public override IEnumerable<DeparturePlace> All()
        {
            PropertyTableWorker propWorker = new PropertyTableWorker(ProductDbContext);

            var a = from p in propWorker.FindBy(PropertyPath.出发城市)
                    select new DeparturePlace
                    {
                        DeparturePlaceId = p.PropertyId,
                        Name = p.PropertyName,
                    };
            return a;
        }

        public override DeparturePlace Find(int id)
        {
            var a = this.All().Single(dp => dp.DeparturePlaceId == id);
            return a;
        }
    }
}
