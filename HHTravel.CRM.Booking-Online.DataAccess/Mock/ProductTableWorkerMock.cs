using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.DataAccess.TableWorkers;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using System.Data.Entity;
using HHTravel.CRM.Booking_Online.Entity;
using HHTravel.CRM.Booking_Online.DataAccess.Repository;

namespace HHTravel.CRM.Booking_Online.DataAccess.Mock
{
    internal class ProductTableWorkerMock : ProductTableWorker
    {
        static List<Product> s_list;
        static List<Product_Property> s_ppList;

        public ProductTableWorkerMock()
            : base(DbContextFactory.Create<ProductDbEntities>())
        {

        }

        public ProductTableWorkerMock(ProductDbEntities productDbContext)
            : base(productDbContext)
        {
        }

        public override Product Find(string productNo)
        {
            var a = (from p in s_list
                    where p.ProductNo == productNo
                    select p).SingleOrDefault();
            return a;
        }

        //基本的join和过滤
        public override IQueryable<Product> All()
        {
            Func<List<Product>> build = () =>
            {
                List<Product> list = new List<Product>();
                string desp = "abcdefghijklmnopqrstuvwxyz....abcdefghijklmnopqrstuvwxyz....abcdefghijklmnopqrstuvwxyz....abcdefghijklmnopqrstuvwxyz....";
                var listGroup = new DestinationGroupRepository(ProductDbContext).All();
                DateTime beginDate = DateTime.Parse("2012-08-01");
                DateTime endDate = DateTime.Parse("2013-12-12");

                Product p;
                s_ppList = new List<Product_Property>();
                for (int i = 1; i <= 100; i++)
                {
                    int productId = i;
                    string productNo = string.Format("FN88{0:D3}", i);
                    var group = MockHelper.GetRandomItem<HHTravel.CRM.Booking_Online.Model.DestinationGroup>(i / 5, listGroup);
                    var region = MockHelper.GetRandomItem<HHTravel.CRM.Booking_Online.Model.DestinationRegion>(i / 7, group.RegionList);
                    // random build a Product_NoDeparture collection
                    List<Product_NoDeparture> listPnd = new List<Product_NoDeparture>();
                    var dateList = MockHelper.GetRandomDateList(beginDate, endDate);
                    foreach (var date in dateList)
                    {
                        Product_NoDeparture pnd = new Product_NoDeparture
                        {
                            ProductId = productId,
                            DepartureDate = MockHelper.GetRandomDate(beginDate, endDate),
                        };
                        listPnd.Add(pnd);
                    }

                    p = new Product
                    {
                        ProductId = productId,
                        ProductNo = productNo,
                        StartCity = MockHelper.GetRandomItem<string>(i, new string[] { "北京", "上海", "广州", "成都" }),
                        Destination1 = group.Name,
                        TripType = (short)MockHelper.GetRandomItem<int>(i / 3, new int[] { 3, 1, 2 }),
                        TripDays = (short)MockHelper.GetRandomItem<int>(i / 4, new int[] { 5, 10, 12, 28 }),
                        ProductFeature = desp,
                        Product_NoDeparture = listPnd,
                    };
                    p.Destination2 = (region != null) ? region.Name : "";
                    p.ProductName = string.Format("[{0}]{1}{2}{3}",
                                            p.ProductId,
                                            p.Destination1,
                                            (p.Destination2 ?? "!分组下未配置区域"),
                                            ((Model.TravelType)p.TripType.Value).ToString());

                    list.Add(p);
                    // add to cached propertyList
                    AddToProjectPropertyList(p, group, region);
                }

                return list;
            };

            if (s_list == null)
            {
                s_list = build();
            }

            return s_list as IQueryable<Product>;
        }

        public override IQueryable<Product> FindByProperty(int propertyId)
        {
            var a = from p in this.All()
                    join prodprop in s_ppList
                    on p.ProductId equals prodprop.ProductId
                    where prodprop.PropertyId == propertyId
                    select p;

            return a;
        }

        /// <summary>
        /// eg.取订购须知时，随机取任一个产品的订购须知
        /// </summary>
        /// <param name="productId">!Ignore</param>
        /// <param name="infoTypeName"></param>
        /// <returns></returns>
        internal override string GetInfoByInfoType(int productId, string infoTypeName)
        {
            var a = from pi in ProductDbContext.Product_Info
                    where pi.InfoTypeName == infoTypeName
                    select pi.Info;
            return MockHelper.GetRandomItem<string>(a);
        }

        protected override int GetSpecsMinPrice(Entity.Product product)
        {
            int[] prices = new int[] { 12000, 180000, 19800000 };
            var price = MockHelper.GetRandomItem<int>(product.ProductId / prices.Length, prices);
            return price;
        }

        internal override List<DateTime> GetDepartureDateList(Entity.Product product)
        {
            // todo: 目前未与product关联
            DateTime beginDate = DateTime.Parse("2012-08-01");
            DateTime endDate = DateTime.Parse("2013-12-12");
            var list = MockHelper.GetRandomDateList(beginDate, endDate).OrderBy(p => p.Date).ToList();
            return list;
        }

        static void AddToProjectPropertyList(Product p,
            HHTravel.CRM.Booking_Online.Model.DestinationGroup group,
            HHTravel.CRM.Booking_Online.Model.DestinationRegion region)
        {
            Action<int, int> addToProductPropertyList = (productId, propertyId) =>
            {
                s_ppList.Add(new Product_Property
                {
                    ProductId = productId,
                    PropertyId = propertyId,
                });
            };

            addToProductPropertyList(p.ProductId, (int)p.TripType);
            // todo: 出发地用对应数值替换掉
            //addToProductPropertyList(p.ProductId, p.StartCity);
            // todo: 
            //foreach (var interest in p.InterestList)
            //{
            //    addToProductPropertyList(p.ProductId, interest.InterestId);
            //}
            addToProductPropertyList(p.ProductId, group.GroupId);
            if (region != null)
            {
                addToProductPropertyList(p.ProductId, region.RegionId);
            }
        }
    }
}
