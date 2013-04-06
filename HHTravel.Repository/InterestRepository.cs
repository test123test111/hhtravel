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