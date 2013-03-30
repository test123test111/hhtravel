using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Model;
using Microsoft.Practices.Unity;
using System.Data.Entity;
using HHTravel.CRM.Booking_Online.Entity;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.TableWorkers;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;

namespace HHTravel.CRM.Booking_Online.DataAccess.Repository
{
    public class DestinationGroupRepository : RepositoryBase<DestinationGroup>, IDestinationGroupRepository
    {
        [InjectionConstructor]
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
            PropertyTableWorker propWorker = new PropertyTableWorker(ProductDbContext);
            PictureTableWorker picWorker = new PictureTableWorker(ProductDbContext);

            // linq to e
            var a = from p in propWorker.FindBy(PropertyPath.一级目的地)
                    join picture in picWorker.FindBy("属性", "营销图")  //todo: 测试是否被缓存
                    on p.PropertyId equals picture.ObjectId
                    select new
                    {
                        DestinationGroup = new DestinationGroup
                            {
                                GroupId = p.PropertyId,
                                Name = p.PropertyName,
                                TopImage = new ImageInfo { Title = picture.Title, Url = picture.URL }
                            },
                        PropertyPath = p.PropertyPath
                    };

            var b = a.ToList();
            b.ForEach(g =>
            {
                var ba = from p in propWorker.FindBy(g.PropertyPath)
                         //join picture in repoPic.FindBy("属性", "营销图")  //todo: 测试是否被缓存
                         //on p.PropertyId equals picture.ObjectId
                         orderby p.SeqNo
                         select new DestinationRegion
                         {
                             RegionId = p.PropertyId,
                             Name = p.PropertyName,
                             //TopImage = new ImageInfo { Title = picture.Title, Url = picture.URL }
                         };
                var bb = ba.ToList();
                g.DestinationGroup.RegionList = bb;
            });
            var c = b.Select(g => g.DestinationGroup);
            return c;
        }

        public override DestinationGroup Find(int id)
        {
            PropertyTableWorker propWorker = new PropertyTableWorker(ProductDbContext);
            PictureTableWorker picWorker = new PictureTableWorker(ProductDbContext);

            var pGroup = propWorker.Find(id);
            var a = from picture in picWorker.FindBy("属性", "营销图")  //todo: 测试是否被缓存
                    where picture.ObjectId == id
                    select new
                    {
                        DestinationGroup = new DestinationGroup
                            {
                                GroupId = pGroup.PropertyId,
                                Name = pGroup.PropertyName,
                                TopImage = new ImageInfo { Title = picture.Title, Url = picture.URL },
                            },
                        RegionList = from p in propWorker.FindBy(pGroup.PropertyPath)
                                     //join picture in repoPic.FindBy("属性", "营销图")
                                     //on p.PropertyId equals picture.ObjectId
                                     orderby p.SeqNo
                                     select new DestinationRegion
                                     {
                                         RegionId = p.PropertyId,
                                         Name = p.PropertyName,
                                         //TopImage = new ImageInfo { Title = picture.Title, Url = picture.URL },
                                     }

                    };
            var b = a.Single();
            b.DestinationGroup.RegionList = b.RegionList.ToList();
            var group = b.DestinationGroup;
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
