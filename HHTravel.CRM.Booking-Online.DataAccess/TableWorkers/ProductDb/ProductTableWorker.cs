using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Entity;
using System.Data.Entity;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;
using HHTravel.CRM.Booking_Online.Model.Exceptions;

namespace HHTravel.CRM.Booking_Online.DataAccess.TableWorkers
{
    internal class ProductTableWorker : ProductDbTableWorkerBase<Product>
    {

        public ProductTableWorker()
            : base(DbContextFactory.Create<ProductDbEntities>())
        {

        }

        public ProductTableWorker(ProductDbEntities productDbContext)
            : base(productDbContext)
        {
        }

        public override Product Find(int id)
        {
            var a = (from p in ProductDbContext.Product
                     where p.ProductId == id
                     select p).SingleOrDefault();
            return a;
        }
        /// <summary>
        /// 根据产品编号查找产品
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public virtual Product Find(string productNo)
        {
            var a = (from p in ProductDbContext.Product
                     where p.ProductNo == productNo
                     select p).SingleOrDefault();
            return a;
        }

        /// <summary>
        /// 所有有效的产品
        /// 有有效日期范围、有价格、价格有有效范围、上架、有效
        /// </summary>
        /// <returns></returns>
        public override IQueryable<Product> All()
        {
            DateTime now = DateTime.Now;
            var a = from p in ProductDbContext.Product
                    where p.ResourceType == (int)ProductResourceType.产品
                        && p.ExpireDate.HasValue
                        && p.EffectDate.HasValue
                        && p.ExpireDate.Value >= p.EffectDate.Value
                        && p.IsUp.HasValue && p.IsUp.Value
                        && p.IsValid.HasValue && p.IsValid.Value
                        && p.Product_Price.Any(pr => pr.Price > 0)
                    //&& pr.ExpireDate >= now)
                    select p;
            return a;
        }

        public virtual IQueryable<Product> AllTickets()
        {
            DateTime now = DateTime.Now;
            var a = from p in ProductDbContext.Product
                    where p.ResourceType == (int)ProductResourceType.航班
                        //&& p.ExpireDate.HasValue
                        //&& p.EffectDate.HasValue
                        //&& p.ExpireDate.Value > p.EffectDate.Value
                        //&& p.IsUp.HasValue && p.IsUp.Value    // 机票产品没有上架/下架操作
                        && p.IsValid.HasValue && p.IsValid.Value
                    //&& p.Product_Price.Any(pr => pr.Price > 0)
                    select p;
            return a;
        }

        public virtual IQueryable<Product> AllHotels()
        {
            DateTime now = DateTime.Now;
            var a = from p in ProductDbContext.Product
                    where p.ResourceType == (int)ProductResourceType.酒店
                        //&& p.ExpireDate.HasValue
                        //&& p.EffectDate.HasValue
                        //&& p.ExpireDate.Value > p.EffectDate.Value
                        && p.IsUp.HasValue && p.IsUp.Value
                        && p.IsValid.HasValue && p.IsValid.Value
                    //&& p.Product_Price.Any(pr => pr.Price > 0)
                    select p;
            return a;
        }

        public virtual IQueryable<Product> FindByProperty(int propertyId)
        {
            var a = FindByProperty(this.All(), propertyId);
            return a;
        }

        /// <summary>
        /// 根据属性Id查找产品
        /// </summary>
        /// <param name="products"></param>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        public virtual IQueryable<Product> FindByProperty(IQueryable<Product> products, int propertyId)
        {
            var a = from p in products
                    join prodprop in ProductDbContext.Product_Property
                    on p.ProductId equals prodprop.ProductId
                    where prodprop.PropertyId == propertyId
                    select p;

            return a;
        }
        /// <summary>
        /// 根据travelType查找产品
        /// </summary>
        /// <param name="products"></param>
        /// <param name="travelTypeId"></param>
        /// <returns></returns>
        public IQueryable<Product> FindByTravelType(IQueryable<Product> products, int travelTypeId)
        {
            var a = from p in products
                    where p.TripType == travelTypeId
                    select p;
            return a;
        }
        /// <summary>
        /// 根据出发地名称查找产品
        /// </summary>
        /// <param name="products"></param>
        /// <param name="departureCity"></param>
        /// <returns></returns>
        public IQueryable<Product> FindByDepartureCity(IQueryable<Product> products, string departureCity)
        {
            var a = from p in products
                    where p.StartCity == departureCity
                    select p;
            return a;
        }
        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        protected virtual IQueryable<Product_Info> GetProductInfos(int productId)
        {
            // todo: cache?
            var a = from i in ProductDbContext.Product_Info
                    where i.ProductId == productId
                    select i;
            return a;
        }
        /// <summary>
        /// 根据目的地分组名称查找产品
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        internal IQueryable<Product> FindByDestinationGroup(string groupName)
        {
            DateTime now = DateTime.Now;
            var a = (from p in this.All()
                     where p.Destination1 == groupName
                     select p);
            return a;
        }
        /// <summary>
        /// 根据目的地区域名称查找产品
        /// </summary>
        /// <param name="regionName"></param>
        /// <returns></returns>
        internal IQueryable<Product> FindByDestinationRegion(string regionName)
        {
            DateTime now = DateTime.Now;
            var a = (from p in this.All()
                     where p.Destination2 == regionName
                     select p);
            return a;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        internal virtual IEnumerable<Product> Page(IEnumerable<Product> list, Model.Pager pager)
        {
            // 排序
            if (pager.SortRule == Model.SortRule.ProductTripDays)
            {
                list = pager.Descending ? list.OrderByDescending(p => p.TripDays)
                    : list.OrderBy(p => p.TripDays);
            }
            else if (pager.SortRule == Model.SortRule.ProductMinDepartureDate)
            {
                list = pager.Descending ? list.OrderByDescending(p => GetDepartureDateList(p).Min())
                    : list.OrderBy(p => GetDepartureDateList(p).Min());
            }
            else if (pager.SortRule == Model.SortRule.ProductMinPrice)
            {
                list = pager.Descending ? list.OrderByDescending(GetSpecsMinPrice)
                    : list.OrderBy(GetSpecsMinPrice);
            }
            else if (pager.SortRule == Model.SortRule.ProductDefault)
            {
                list = list.OrderByDescending(p => p.Weight)
                                .ThenBy(p => GetDepartureDateList(p).Min())
                                .ThenBy(p => p.TripDays);
            }
            else
            {
                throw new ArgumentOutOfRangeException("pager.SortRule");
            }

            int pageCount;
            var b = PagerHelper.TakePage(list, pager.PageIndex, out pageCount);
            pager.PageCount = pageCount;

            return b;

        }

        /// <summary>
        /// 根据infoTypeName获取信息
        /// todo: 优化
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="infoTypeName"></param>
        /// <returns></returns>
        internal virtual string GetInfoByInfoType(int productId, string infoTypeName)
        {
            var a = (from info in this.GetProductInfos(productId)
                     where info.InfoTypeName == infoTypeName
                     select info.Info).ToArray();
            //var b = a.FirstOrDefault(i => i.InfoTypeName == infoTypeName);
            var b = string.Join("<br/>", a);
            return b;
        }

        /// <summary>
        /// 获取指定产品所有规格的价格中，最低的价格。
        /// 没有任何价格的返回null。
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        protected virtual int GetSpecsMinPrice(Product product)
        {
            int price;
            var pi = GetSpecsMinPriceItem(product);
            price = (pi != null) ? pi.Price : 0;

            return price;
        }
        /// <summary>
        /// 获取指定产品所有规格的价格中，最低的价格
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        internal virtual Product_Price GetSpecsMinPriceItem(Product product)
        {
            var a = (from p in product.Product_Price
                     orderby p.Price
                     select p).FirstOrDefault();
            return a;
        }

        /// <summary>
        /// 获取出发日期
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        internal virtual List<DateTime> GetDepartureDateList(Product product)
        {
            List<DateTime> list = new List<DateTime>();

            if (!product.EffectDate.HasValue || !product.ExpireDate.HasValue)
            {
                throw new DataException(string.Format("产品{0}未定义有效起止时间", product.ProductId));
            }
            var beginDate = product.EffectDate.Value;
            var endDate = product.ExpireDate.Value;
            if (beginDate > endDate)
            {
                throw new DataException(string.Format("产品{0}定义了错误的有效起止时间", product.ProductId));
            }

            var delta = endDate - beginDate;
            var days = delta.TotalDays + 1;
            for (int i = 0; i < days; i++)
            {
                DateTime d = product.EffectDate.Value.AddDays(i);
                // 排除掉不出发的日期
                if (!product.Product_NoDeparture.Any(nd => nd.DepartureDate.Date == d.Date))
                {
                    list.Add(d);
                }
            }

            return list;
        }
    }
}
