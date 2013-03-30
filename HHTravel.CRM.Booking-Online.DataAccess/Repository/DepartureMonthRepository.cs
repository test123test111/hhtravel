using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Model;
using HHTravel.CRM.Booking_Online.IRepository;
using System.Data.Entity;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using Microsoft.Practices.Unity;
using HHTravel.CRM.Booking_Online.DataAccess.TableWorkers;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;

namespace HHTravel.CRM.Booking_Online.DataAccess.Repository
{
    public class DepartureMonthRepository : RepositoryBase<DepartureMonth>, IDepartureMonthRepository
    {
        [InjectionConstructor]
        public DepartureMonthRepository()
        {
            this.ProductDbContext = DbContextFactory.Create<ProductDbEntities>();
        }

        internal DepartureMonthRepository(ProductDbEntities productDbContext)
        {
            this.ProductDbContext = productDbContext;
        }

        public override IEnumerable<DepartureMonth> All()
        {
            PropertyTableWorker propWorker = new PropertyTableWorker(ProductDbContext);

            var a = from p in propWorker.FindBy(PropertyPath.出发月份)
                    select new DepartureMonth
                    {
                        MonthId = p.PropertyId,
                        Name = p.PropertyName,
                    };
            var b = a.ToList();

            b.ForEach(dm => {
                var name = dm.Name;
                dm.Year = int.Parse(name.Substring(0, 4));
                dm.Month = int.Parse(name.Substring(name.IndexOf('-') + 1, name.IndexOf('月') - name.IndexOf('-') - 1));
            });

            return b;
        }

        public override DepartureMonth Find(int id)
        {
            var a = this.All().Single(dm => dm.MonthId == id);
            return a;
        }
    }
}
