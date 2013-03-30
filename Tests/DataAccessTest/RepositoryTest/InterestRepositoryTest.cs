using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HHTravel.CRM.Booking_Online.DataAccess.Repository;
using HHTravel.CRM.Booking_Online.IRepository;

namespace DataAccessTest
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
