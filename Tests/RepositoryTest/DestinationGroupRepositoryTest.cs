using System.Linq;
using HHTravel.CRM.Booking_Online.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RepositoryTest
{
    [TestClass]
    public class DestinationGroupRepositoryTest : RepositoryTestBase
    {
        [TestMethod]
        public void AllTest()
        {
            var repo = new DestinationGroupRepository();
            var a = repo.All();

            //Assert.IsTrue(a.Count() > 0);
            var b = a.ToList();
            Assert.IsTrue(b.Count > 0, "没有获取到DestinationGroup");
            Assert.IsTrue(b[0].RegionList.ToList().Count > 0, "没有获取到DestinationRegion");
        }

        [TestMethod]
        public void FindTest()
        {
            var groupId = 46;
            var repo = new DestinationGroupRepository();
            var a = repo.Find(groupId);
            var b = a.RegionList;

            Assert.IsTrue(b.Count() > 0);
            Assert.IsTrue(b.ToList().Count > 0);

            // todo:
            var c = new DestinationGroupRepository().All();
            var d = c.Where(g => groupId == g.GroupId).Single();
            var e = d.RegionList;

            Assert.IsTrue(e.Count() > 0);
            Assert.IsTrue(e.ToList().Count > 0);
        }

        [TestMethod]
        public void ForeachAllRegionTest()
        {
            var repo = new DestinationGroupRepository();
            var a = repo.All().ToList();

            foreach (var g in a)
            {
                Assert.IsTrue(g.RegionList.Count > 0, "数据不完整");
                foreach (var b in g.RegionList)
                {
                    Assert.IsNotNull(b);
                }
            }
        }
    }
}