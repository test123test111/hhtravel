using System.Linq;
using HHTravel.CRM.Booking_Online.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RepositoryTest
{
    [TestClass]
    public class InterestRepositoryTest : RepositoryTestBase
    {
        [TestMethod]
        public void AllTest()
        {
            var repo = new InterestRepository();
            var a = repo.All();

            Assert.IsTrue(a.ToList().Count > 0);
        }
    }
}