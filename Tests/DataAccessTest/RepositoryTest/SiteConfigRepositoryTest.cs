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
    public class SiteConfigRepositoryTest : RepositoryTestBase
    {
        /// <summary>
        ///A test for FindBy
        ///</summary>
        [TestMethod()]
        public void FindByTest()
        {
            SiteConfigRepository target = new SiteConfigRepository(); // TODO: Initialize to an appropriate value

            SiteConfig actual;
            actual = target.GetSiteConfig();

            Assert.IsNotNull(actual.TopImageAboutUs);
            Assert.IsNotNull(actual.TopImageDeparture);
            Assert.IsNotNull(actual.TopImageDestination);
            Assert.IsNotNull(actual.TopImageInterest);
            Assert.IsNotNull(actual.TopImageUnique);

            Assert.IsNotNull(actual.TopImageAroundWorld);
        }
    }
}
