using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Model;
using HHTravel.CRM.Booking_Online.IRepository;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.TableWorkers;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;

namespace HHTravel.CRM.Booking_Online.DataAccess.Repository
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
            PropertyTableWorker propWorker = new PropertyTableWorker(ProductDbContext);
            PictureTableWorker picWorker = new PictureTableWorker(ProductDbContext);

            var a = from p in propWorker.FindBy(PropertyPath.产品主题)
                    join picture in picWorker.FindBy("属性", "营销图")  //todo: 测试是否被缓存
                    on p.PropertyId equals picture.ObjectId
                    select new Interest
                    {
                        InterestId = p.PropertyId,
                        Name = p.PropertyName,
                        TopImage = new ImageInfo { Title = picture.Title, Url = picture.URL },
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
