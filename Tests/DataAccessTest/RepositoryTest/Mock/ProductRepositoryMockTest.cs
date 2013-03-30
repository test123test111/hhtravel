using System;
using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess.Mock;
using HHTravel.CRM.Booking_Online.DataAccess.Repository;
using HHTravel.CRM.Booking_Online.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessTest.RepositoryTest.Mock
{
    [TestClass]
    public class ProductRepositoryMockTest : RepositoryTestBase
    {
        ProductRepositoryMock _repoMock = new ProductRepositoryMock();

        Pager _pager = new Pager();


        //[TestMethod]
        //public void RefreshCacheTest()
        //{
        //    _repoMock.RefreshCache();
        //    Assert.IsTrue(true);
        //}

        [TestMethod]
        public void FindTest()
        {
            int productId = 0;
            var a = _repoMock.Find(productId);

            Assert.IsNotNull(a);
        }

        [TestMethod]
        public void AllTest()
        {
            var a = _repoMock.All();
            var pl = a.ToList();
            Assert.IsTrue(pl.Count > 0);

            foreach (var p in pl)
            {
                Assert.IsTrue(p.ProductId > 0);
                Assert.IsTrue(p.ProductName.Length > 0);
                Assert.IsTrue(p.ProductNo.Length > 0);
                Assert.IsNotNull(p.TravelType);
                // DepartureDateList
                Assert.IsTrue(p.DepartureDateList.Count() > 0);
                // ScheduleItemList
                // GoFilghtList
                // ReturnFilghtList
                // HotelList
                Assert.IsTrue(p.InterestList.Count() > 0);
            }
        }

        /// <summary>
        /// 是否存在单品游
        /// </summary>
        [TestMethod]
        public void AllTest2()
        {
            foreach (var p in _repoMock.All())
            {
                if (p.TravelType == TravelType.单品游)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }

            Assert.IsTrue(false);
        }

        [TestMethod]
        public void SearchTest()
        {
            var travelTypeIds = Enum.GetValues(typeof(TravelType)).Cast<int?>();
            var departurePlaceNames = new DepartureCityRepository().All().Select(dp => dp.Name);
            var groupIds = new DestinationGroupRepository().All().Select(g => g.GroupId);
            var interestIds = new InterestRepository().All().Select(dp => (int?)dp.InterestId);
            int max = 1000;
            int count = 0;
            for (int i = 0; i < max; i++)
            {
                // 业务规则：搜索时，group 不能为null 
                var groupId = MockHelper.GetRandomItem<int>(groupIds);
                var group = new DestinationGroupRepository().Find(groupId);
                var regionIds = group.RegionList.Select(r => (int?)r.RegionId);

                var searchCondition = new
                {
                    DeparturePlace = MockHelper.GetRandomItem<string>(departurePlaceNames, true),
                    TravelType = MockHelper.GetRandomItem<int?>(travelTypeIds, true),
                    Interest = MockHelper.GetRandomItem<int?>(interestIds, true),
                    DestinationGroup = groupId,
                    DestinationRegion = MockHelper.GetRandomItem<int?>(regionIds, true),
                };
                
                var pl = _repoMock.Search(searchCondition.DeparturePlace,
                    searchCondition.DestinationGroup,
                    searchCondition.DestinationRegion,
                    searchCondition.TravelType,
                    null,
                    null,
                    searchCondition.Interest,
                    _pager);

                if (_pager.PageCount > 0)
                {
                    count++;
                    Console.WriteLine("第{0}次搜索命中", i);
                }
            }

            Console.WriteLine("命中率: {0}/{1}", count, max);
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void FindByInterestTest()
        {
            var i = MockHelper.GetRandomItem<Interest>(new InterestRepository().All());
            var pl = _repoMock.FindByInterest(i.InterestId, _pager);

            Assert.IsTrue(_pager.PageCount > 0);
        }

        [TestMethod]
        public void FindByDestinationGroupTest()
        {
            var g = MockHelper.GetRandomItem<DestinationGroup>(new DestinationGroupRepository().All());
            var pl = _repoMock.FindByDestinationGroup(g.GroupId, _pager);

            Assert.IsTrue(_pager.PageCount > 0);
        }

        [TestMethod]
        public void FindByDestinationRegionTest()
        {
            var g = MockHelper.GetRandomItem<DestinationGroup>(new DestinationGroupRepository().All());
            var r = MockHelper.GetRandomItem<DestinationRegion>(g.RegionList);
            var pl = _repoMock.FindByDestinationRegion(r.RegionId, _pager);

            Assert.IsTrue(_pager.PageCount > 0);
        }

        [TestMethod]
        public void FindByDepartureDateTest()
        {
            DateTime beginDate = new DateTime(2012, 8, 1);
            DateTime endDate = beginDate.AddMonths(1);
            //var a = _repo.FindByDepartureDate(beginDate, endDate); 

            var a = from p in _repoMock.All()
                    where p.DepartureDateList.Any(d => d >= beginDate && d <= endDate)
                    select p;

            var b = a.ToList();

            Assert.IsTrue(b.Count > 0);
        }

        [TestMethod]
        public void FindByTrvaelTypeTest()
        {
            var travelTypes = Enum.GetValues(typeof(TravelType)).Cast<TravelType>();
            var tt = MockHelper.GetRandomItem<TravelType>(travelTypes);
            var pl = _repoMock.FindByTravelType((int)tt, _pager);

            Assert.IsTrue(_pager.PageCount > 0);
        }

        [TestMethod]
        public void FindByDeparturePlaceTest()
        {
            var tt = MockHelper.GetRandomItem<DepartureCity>(new DepartureCityRepository().All());
            var pl = _repoMock.FindByDepartureCity(tt.Name, _pager);

            Assert.IsTrue(_pager.PageCount > 0);
        }

        [TestMethod]
        public void GetPhotoListTest()
        {
            // 随机获取一个产品
            var product = MockHelper.GetRandomItem<Product>(_repoMock.All());

            Assert.IsNotNull(product.PhotoList);
            Assert.IsTrue(product.PhotoList.Count > 0);
        }
    }
}
