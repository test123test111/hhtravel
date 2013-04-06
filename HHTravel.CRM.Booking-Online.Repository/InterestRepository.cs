﻿using System.Collections.Generic;
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
    public class InterestRepository : RepositoryBase<Interest>, IInterestRepository
    {
        [InjectionConstructor]
        public InterestRepository()
        {
            this.ProductDbContext = DbContextFactory.Create<ProductDbEntities>();
        }

        internal InterestRepository(ProductDbEntities productDbContext)
        {
            this.ProductDbContext = productDbContext;
        }

        /// <summary>
        /// 获取旅行主题分组
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Interest> All()
        {
            PropertiesProvider propProvider = new PropertiesProvider(ProductDbContext);

            var a = from p in propProvider.FindBy(PropertyPath.产品主题)
                    orderby p.SeqNo
                    select new Interest
                    {
                        InterestId = p.PropertyId,
                        Name = p.PropertyName,
                    };
            return a;
        }

        public override Interest Find(int id)
        {
            var interest = this.All().Single(i => i.InterestId == id);
            return interest;
        }
    }
}