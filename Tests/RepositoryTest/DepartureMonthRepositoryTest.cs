using System.Linq;
using HHTravel.CRM.Booking_Online.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RepositoryTest
{
    [TestClass()]
    public class DepartureMonthRepositoryTest : RepositoryTestBase
    {
        /// <summary>
        ///A test for All
        ///</summary>
        [TestMethod()]
        public void AllTest()
        {
            var target = new DepartureMonthRepository(); // TODO: Initialize to an appropriate value

            var actual = target.All();
            Assert.IsTrue(actual.Count() > 0);
        }
    }
}