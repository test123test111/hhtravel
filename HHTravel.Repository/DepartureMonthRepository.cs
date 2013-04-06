using System.Collections.Generic;
using System.Linq;
using HHTravel.DataAccess;
using HHTravel.DataAccess.DbContexts;
using HHTravel.DataAccess.HardCode;
using HHTravel.DataAccess.Providers;
using HHTravel.DomainModel;
using HHTravel.IRepository;
using Microsoft.Practices.Unity;

namespace HHTravel.Repository
{
    public class DepartureMonthRepository : RepositoryBase<DepartureMonth>, IDepartureMonthRepository
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), InjectionConstructor]
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
            PropertiesProvider propProvider = new PropertiesProvider(ProductDbContext);

            var a = from p in propProvider.FindBy(PropertyPath.出发年月)
                    select new DepartureMonth
                    {
                        MonthId = p.PropertyId,
                        Name = p.PropertyName,
                    };
            var b = a.ToList();

            b.ForEach(dm =>
            {
                var name = dm.Name;
                dm.Year = int.Parse(name.Substring(0, 4));
                dm.Month = int.Parse(name.Substring(name.IndexOf('-') + 1, name.IndexOf('月') - name.IndexOf('-') - 1));
            });
            b = b.OrderBy(dm => dm.Year).ThenBy(dm => dm.Month).ToList();
            return b;
        }

        public override DepartureMonth Find(int id)
        {
            var a = this.All().Single(dm => dm.MonthId == id);
            return a;
        }
    }
}