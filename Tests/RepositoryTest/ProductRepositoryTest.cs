using System;
using System.Collections.Generic;
using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess.Providers;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RepositoryTest
{
    [TestClass]
    public class ProductRepositoryTest : RepositoryTestBase
    {
        private Pager _pager = new Pager();
        private ProductRepository _repo = new ProductRepository();

        [TestMethod]
        public void AllLoadTest()
        {
            var a = _repo.All();

            //Assert.IsTrue(a.Count() > 0);

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Assert.IsTrue(a.ToList().Count > 0);
            sw.Stop();
            Console.WriteLine("1： " + sw.ElapsedMilliseconds);

            for (int i = 2; i < 11; i++)
            {
                sw.Restart();
                a = _repo.All();
                Assert.IsTrue(a.ToList().Count > 0);
                sw.Stop();

                Console.WriteLine(i + "： " + sw.ElapsedMilliseconds);
            }
        }

        /// <summary>
        ///A test for FindByProductName
        ///</summary>
        [TestMethod()]
        public void FindByProductNameTest()
        {
            IProductRepository repo = new ProductTempRepository();
            string productName = "伦敦 + 巴黎专属车导TravelType2 9 天";
            Pager pager = null;

            IEnumerable<Product> actual;
            actual = repo.FindByProductName(productName, pager);
            Assert.IsTrue(actual.Count() > 0);
        }

        [TestMethod]
        public void FindTest()
        {
            // 输入一个不存在的Id
            int productId = 649;
            var a = _repo.Find(productId);

            Assert.IsNull(a);
        }

        #region 数据校验工具

        //[TestMethod]
        //public void AllHasPriceTest()
        //{
        //    var a = _repo.All();
        //    Assert.IsTrue(a.Any(p => p.Product_Price.Count > 0));
        //    var b = a.Where(p => p.Product_Price.Count > 0).Select(p => p.Product_Price);
        //}

        [TestMethod]
        public void AllShouldHaveMainImageTest()
        {
            var a = _repo.All().ToList();
            var b = from p in a
                    where p.MainImage == null
                    select p;

            Assert.IsTrue(b.Count() == 0, "AllShouldHaveMainImage");
        }

        #endregion 数据校验工具

        #region 搜索过滤相关

        [TestMethod]
        public void FindByDepartureDateTest()
        {
            DateTime beginDate = new DateTime(2012, 9, 1);
            DateTime endDate = beginDate.AddMonths(1);
            var a = _repo.FindByDepartureDate(beginDate, endDate, _pager);
            var b = a.ToList();

            Assert.IsTrue(b.Count > 0);
            var expected = a.Select(p => !p.ProductName.StartsWith("old")).Count();
            Assert.IsTrue(expected > 0);
        }

        [TestMethod]
        public void FindByDestinationGroupTest()
        {
            var gRepo = new DestinationGroupRepository().All().Where(g => g.GroupId == 44).Single(); //欧洲
            var a = _repo.FindByDestinationGroup(gRepo.GroupId, _pager);

            Assert.IsTrue(_pager.PageCount > 0);
            var expected = a.Select(p => !p.ProductName.StartsWith("old")).Count();
            Assert.IsTrue(expected > 0);
        }

        [TestMethod]
        public void FindByInterestTest()
        {
            var interest = new InterestRepository().All().Where(i => i.InterestId == 37).Single();  //岛屿度假
            var a = _repo.FindByInterest(interest.InterestId, _pager);

            Assert.IsTrue(_pager.PageCount > 0);
            var expected = a.Select(p => !p.ProductName.StartsWith("old")).Count();
            Assert.IsTrue(expected > 0);
        }

        [TestMethod]
        public void SearchTest()
        {
            throw new NotImplementedException();
        }

        #endregion 搜索过滤相关

        #region 获取关联属性

        /// <summary>
        ///A test for GetDepartureDateList
        ///</summary>
        [TestMethod()]
        public void GetDepartureDateListTest()
        {
            // sql: 有关闭日期的产品
            //select pd.ProductId, COUNT(1) from Product_NoDeparture pd
            //left join Product p
            //on pd.ProductId = p.ProductId
            //group by pd.ProductId

            // sql: 有价格的产品
            //select pp.ProductId, COUNT(1) from Product_Price pp
            //left join Product p
            //on pp.ProductId = p.ProductId
            //group by pp.ProductId

            Action<bool, HHTravel.CRM.Booking_Online.Entity.Product> assert = (expected, product) =>
            {
                Assert.IsTrue(expected, string.Format("productId: {0}", product.ProductId));
            };

            ProductRepository target = new ProductRepository();

            var all = target.AllWrapperedProducts();
            foreach (HHTravel.CRM.Booking_Online.Entity.ProductWrapper pw in all)
            {
                var product = pw.Product;
                Assert.IsNotNull(product);
                Assert.IsNotNull(product.EffectDate);
                Assert.IsNotNull(product.ExpireDate);
                Assert.IsNotNull(product.SetOffDays); // ""
                Assert.IsNotNull(product.Product_Price);
                Assert.IsNotNull(product.Product_NoDeparture);

                bool expected;
                int actual;
                List<PriceDate> list;
                list = target.GetPriceDateList(pw);
                actual = list.Count;

                if (product.Product_Price.Count == 0)
                {
                    expected = actual == 0;
                }
                else
                {
                    DateTime today = DateTime.Now.Date;
                    var maxDateHasPrice = product.Product_Price.Max(pp => pp.ExpireDate);
                    var minDateHasPrice = product.Product_Price.Min(pp => pp.EffectDate);
                    if (product.ExpireDate < product.EffectDate)
                    {
                        assert(expected = actual == 0, product);
                    }
                    else if (product.ExpireDate == product.EffectDate)
                    {
                        assert(expected = actual == 1, product);    // !care: 假定了那一天有价格
                    }
                    else if (maxDateHasPrice < product.EffectDate
                            || minDateHasPrice > product.ExpireDate)
                    {
                        assert(expected = actual == 0, product);
                    }
                    else if (today > product.ExpireDate)
                    {
                        assert(expected = actual == 0, product);
                    }
                    else if (today == product.ExpireDate)
                    {
                        assert(expected = actual == 1, product);    // !care: 假定了那一天有价格
                    }
                    else
                    {
                        int delta = (int)(product.ExpireDate.Value - (today > product.EffectDate.Value ? today : product.EffectDate.Value)).TotalDays + 1;
                        assert(expected = actual <= delta, product);
                    }
                }
            }
        }

        [TestMethod()]
        public void GetFlightsTest()
        {
            var actual = _repo.GetTickets(649);
            foreach (var t in actual)
            {
                Assert.IsTrue(t.FlightList.Count > 0);
            }
        }

        [TestMethod()]
        public void GetHotelsTest()
        {
            ProductRepository repo = new ProductRepository();
            var actual = repo.GetHotels(9).ToList();
            Assert.IsTrue(actual.Count > 0);
        }

        [TestMethod()]
        public void GetPhotosTest()
        {
            var actual = _repo.Find("TD-SH-70078");
            Assert.IsTrue(actual.PhotoList.Count > 0);
        }

        [TestMethod()]
        public void GetHotelSegmentsTest()
        {
            ProductTempRepository repo = new ProductTempRepository();
            var actual = repo.GetHotelSegments(20501).ToList();
            Assert.IsTrue(actual.Count > 0);
        }

        [TestMethod()]
        public void AvailableHotelsTest()
        {
            ProductRepository repo = new ProductRepository();
            //repo
        }

        #endregion 获取关联属性
    }
}