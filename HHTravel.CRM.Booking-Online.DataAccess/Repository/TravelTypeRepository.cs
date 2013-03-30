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
    public class TravelTypeRepository : RepositoryBase<TravelType>, ITravelTypeRepository
    {
        private static List<TravelType> s_list = new List<TravelType> { 
            new TravelType { Name = "团队游", TravelTypeId = 1, },
            new TravelType { Name = "自由行", TravelTypeId = 2, },
            new TravelType { Name = "单品游", TravelTypeId = 3, },
        };

        [InjectionConstructor]
        public TravelTypeRepository()
        {
            this.ProductDbContext = DbContextFactory.Create<ProductDbEntities>();
        }

        internal TravelTypeRepository(ProductDbEntities productDbContext)
        {
            this.ProductDbContext = productDbContext;
        }

        /// <summary>
        /// 获取旅行主题分组
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<TravelType> All()
        {
            //InternalPropertyRepository _repoProperty = new InternalPropertyRepository(DbContext);

            //var a = from p in _repoProperty.FindBy(PropertyPath.线路类型)
            //        select new TravelType
            //        {
            //            TravelTypeId = p.PropertyId,
            //            Name = p.PropertyName,
            //        };
            //return a;
            return s_list;
        }

        public override TravelType Find(int id)
        {
            if (id == 0)
                throw new ArgumentOutOfRangeException("id");

            var a = this.All().Single(i => i.TravelTypeId == id);
            return a;
        }
    }
}
