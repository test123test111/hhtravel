using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HHTravel.CRM.Booking_Online.DataAccess.Repository;
using HHTravel.CRM.Booking_Online.Model;

namespace DataAccessTest.RepositoryTest
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
