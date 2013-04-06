using System.Collections.Generic;
using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;
using HHTravel.CRM.Booking_Online.DataAccess.Providers;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.IRepository;
using Microsoft.Practices.Unity;

namespace HHTravel.CRM.Booking_Online.Repository
{
    public class DestinationGroupRepository : RepositoryBase<DestinationGroup>, IDestinationGroupRepository
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), InjectionConstructor]
        public DestinationGroupRepository()
        {
            this.ProductDbContext = DbContextFactory.Create<ProductDbEntities>();
        }

        internal DestinationGroupRepository(ProductDbEntities productDbContext)
        {
            this.ProductDbContext = productDbContext;
        }

        /// <summary>
        /// 获取目的地分组
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DestinationGroup> All()
        {
            PropertiesProvider propProvider = new PropertiesProvider(ProductDbContext);

            // 匹配所有目的地分组和目的地区域
            string pattern = string.Format("{0}%", (int)PropertyPath.一级目的地);
            var allDestinationList = (from p in propProvider.All()

                                      //where SqlFunctions.PatIndex(pattern, p.ParentPropertyPath) > 0
                                      orderby p.PropertyLevel, p.SeqNo
                                      select new
                                      {
                                          p.PropertyId,
                                          p.PropertyName,
                                          p.ParentPropertyPath,
                                          p.PropertyPath
                                      }).ToList();

            // linq to e
            var a = from d in allDestinationList
                    where d.ParentPropertyPath == ((int)PropertyPath.一级目的地).ToString()  // 匹配所有目的地分组
                    let regions = (from d2 in allDestinationList
                                   where d2.ParentPropertyPath == d.PropertyPath
                                   select new DestinationRegion
                                   {
                                       RegionId = d2.PropertyId,
                                       Name = d2.PropertyName,
                                   })
                    let dGroup = new DestinationGroup
                    {
                        GroupId = d.PropertyId,
                        Name = d.PropertyName,
                        RegionList = regions.ToList()
                    }
                    select dGroup;
            return a;
        }

        public override DestinationGroup Find(int id)
        {
            var group = (from d in this.All()
                         where d.GroupId == id
                         select d).SingleOrDefault();
            return group;
        }

        public virtual DestinationGroup Find(string groupName)
        {
            var group = this.All().Single(g => g.Name == groupName);
            return group;
        }

        public virtual DestinationRegion Find(DestinationGroup group, string regionName)
        {
            var region = group.RegionList.Single(r => r.Name == regionName);
            return region;
        }
    }
}