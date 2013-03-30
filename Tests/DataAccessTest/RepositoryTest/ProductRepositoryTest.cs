using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HHTravel.CRM.Booking_Online.DataAccess.Repository;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;

namespace DataAccessTest
{
    [TestClass]
    public class TravelRepositoryTest : RepositoryTestBase
    {
        ProductRepository _repo = new ProductRepository();
        HHTravel.CRM.Booking_Online.Model.Pager _pager = new HHTravel.CRM.Booking_Online.Model.Pager();

        [TestMethod]
        public void FindTest()
        {
            // 输入一个不存在的Id
            int productId = 649;
            var a = _repo.Find(productId);

            Assert.IsNull(a);
        }

        [TestMethod]
        public void AllLoadTest()
        {
            var a = _repo.TableWorker.All();

            //Assert.IsTrue(a.Count() > 0);


            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Assert.IsTrue(a.ToList().Count > 0);
            sw.Stop();
            Console.WriteLine("1： " + sw.ElapsedMilliseconds);

            for (int i = 2; i < 11; i++)
            {
                sw.Restart();
                a = _repo.TableWorker.All();
                Assert.IsTrue(a.ToList().Count > 0);
                sw.Stop();

                Console.WriteLine(i + "： " + sw.ElapsedMilliseconds);
            }
        }

        #region 数据校验工具
        [TestMethod]
        public void AllShouldHaveMainImageTest()
        {
            var a = _repo.All().ToList();
            var b = from p in a
                    where p.MainImage == null
                    select p;

            Assert.IsTrue(b.Count() == 0, "AllShouldHaveMainImage");
        }

        [TestMethod]
        public void AllShouldHaveTopImageTest()
        {
            var a = _repo.All().ToList();
            var b = from p in a
                    where p.TopImage == null
                    select p;

            Assert.IsTrue(b.Count() == 0, "AllShouldHaveTopImage");
        }

        [TestMethod]
        public void AllHasPriceTest()
        {
            var a = _repo.TableWorker.All();
            Assert.IsTrue(a.Any(p => p.Product_Price.Count > 0));
            var b = a.Where(p => p.Product_Price.Count > 0).Select(p => p.Product_Price);
        }
        #endregion

        #region 搜索过滤相关
        [TestMethod]
        public void SearchTest()
        {
            throw new NotImplementedException();
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
        public void FindByDestinationGroupTest()
        {
            var gRepo = new DestinationGroupRepository().All().Where(g => g.GroupId == 44).Single(); //欧洲
            var a = _repo.FindByDestinationGroup(gRepo.GroupId, _pager);

            Assert.IsTrue(_pager.PageCount > 0);
            var expected = a.Select(p => !p.ProductName.StartsWith("old")).Count();
            Assert.IsTrue(expected > 0);
        }

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
        #endregion

        #region 获取关联属性
        [TestMethod()]
        public void GetHotelsTest()
        {
            ProductRepository repo = new ProductRepository();
            var actual = repo.GetHotels(9).ToList();
            Assert.IsTrue(actual.Count > 0);
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
        public void GetPhotosTest()
        {
            var actual = _repo.Find("TD-SH-70078");
            Assert.IsTrue(actual.PhotoList.Count > 0);
        }

        [TestMethod()]
        public void GetInfoByInfoTypeTest()
        {
            var repo = new ProductRepository_Accessor();
            var actual = repo.TableWorker.GetInfoByInfoType(5, ProductInfoType.订购须知);
            Assert.IsTrue(!string.IsNullOrEmpty(actual));
            actual = repo.TableWorker.GetInfoByInfoType(5, ProductInfoType.费用包含);
            Assert.IsTrue(!string.IsNullOrEmpty(actual));
            actual = repo.TableWorker.GetInfoByInfoType(5, ProductInfoType.费用不包含);
            Assert.IsTrue(!string.IsNullOrEmpty(actual));
        }

        [TestMethod()]
        public void GetRoomClassInfoListTest()
        {
            int productId = 646;
            var roomClasses = _repo.GetRoomClasses(productId);
            Assert.IsTrue(roomClasses.Count() > 0);

            var a = (from r in roomClasses
                     group r by r.RoomClassId into g
                     select new
                     {
                         g.Key,
                         Price = g.Min(p => p.Price),
                     });

            roomClasses = from r in roomClasses
                          join aa in a
                          on r.RoomClassId equals aa.Key
                          where r.Price == aa.Price
                          select r;
            Assert.IsTrue(roomClasses.Count() > 0);
        }

        #endregion
    }
}
