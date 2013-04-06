using System.Collections.Generic;
using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;
using HHTravel.CRM.Booking_Online.DataAccess.Providers;
using HHTravel.CRM.Booking_Online.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessTest
{
    [TestClass()]
    public class PropertiesProviderTest
    {
        private PropertiesProvider _repoProp = new PropertiesProvider();

        /// <summary>
        ///A test for FindBy
        ///</summary>
        [TestMethod()]
        public void FindByTest()
        {
            IEnumerable<Property> actual;
            actual = _repoProp.FindBy(PropertyPath.一级目的地);

            Assert.IsTrue(actual.ToList().Count > 0);
        }
    }
}