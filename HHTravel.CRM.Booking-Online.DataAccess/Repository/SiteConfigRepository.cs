using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Model;
using HHTravel.CRM.Booking_Online.IRepository;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;
using HHTravel.CRM.Booking_Online.DataAccess.TableWorkers;

namespace HHTravel.CRM.Booking_Online.DataAccess.Repository
{
    public class SiteConfigRepository : RepositoryBase<SiteConfig>, ISiteConfigRepository
    {
        [InjectionConstructor]
        public SiteConfigRepository()
        {
            this.ProductDbContext = DbContextFactory.Create<ProductDbEntities>();
        }

        internal SiteConfigRepository(ProductDbEntities productDbContext)
        {
            this.ProductDbContext = productDbContext;
        }

        public SiteConfig GetSiteConfig()
        {
            SiteConfig conf = new SiteConfig();
            PictureTableWorker picWorker = new PictureTableWorker(ProductDbContext);
            PropertyTableWorker propWorker = new PropertyTableWorker(ProductDbContext);

            string[] propertyNames = new string[]{
                // 顶部链接
                PropertyName.品牌理念,
                PropertyName.目的地,
                PropertyName.出发月份,
                PropertyName.旅行主题,
                PropertyName.热门品鉴,

                PropertyName.环游世界,
                // 底部链接
                PropertyName.隐私政策,
                PropertyName.电子报订阅,
                PropertyName.联络我们,
            };

            var a = (from p in picWorker.All()
                     join prop in propWorker.All()
                     on p.ObjectId equals prop.PropertyId
                     where propertyNames.Contains(prop.PropertyName)
                        && p.Location == PictureLocation.营销图
                        && p.ObjectType == PictureObjectType.属性
                     select new
                     {
                         PropertyName = prop.PropertyName,
                         TopImage = new ImageInfo
                         {
                             Title = p.Title,
                             Url = p.URL,
                         }
                     });
            // 执行查询
            var list = a.ToList();

            try
            {
                // 顶部链接
                conf.TopImageAboutUs = list.SingleOrDefault(p => p.PropertyName == PropertyName.品牌理念).TopImage;
                conf.TopImageDestination = list.SingleOrDefault(p => p.PropertyName == PropertyName.目的地).TopImage;
                conf.TopImageDeparture = list.SingleOrDefault(p => p.PropertyName == PropertyName.出发月份).TopImage;
                conf.TopImageInterest = list.SingleOrDefault(p => p.PropertyName == PropertyName.旅行主题).TopImage;
                conf.TopImageUnique = list.SingleOrDefault(p => p.PropertyName == PropertyName.热门品鉴).TopImage;

                conf.TopImageAroundWorld = list.SingleOrDefault(p => p.PropertyName == PropertyName.环游世界).TopImage;
                // 底部链接
                conf.TopImagePrivacyPolicy = list.SingleOrDefault(p => p.PropertyName == PropertyName.隐私政策).TopImage;
                conf.TopImageNewsletter = list.SingleOrDefault(p => p.PropertyName == PropertyName.电子报订阅).TopImage;
                conf.TopImageContactUs = list.SingleOrDefault(p => p.PropertyName == PropertyName.联络我们).TopImage;
            }
            catch (NullReferenceException ex)
            {
                new Exception("获取栏目营销图失败。请检查数据库配置。", ex);
            }

            return conf;
        }

        public override IEnumerable<SiteConfig> All()
        {
            throw new NotImplementedException();
        }

        public override SiteConfig Find(int id)
        {
            throw new NotImplementedException();
        }
    }
}
