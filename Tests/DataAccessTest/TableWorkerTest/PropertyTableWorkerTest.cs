using HHTravel.CRM.Booking_Online.DataAccess.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using HHTravel.CRM.Booking_Online.DataAccess;
using DataAccessTest.RepositoryTest;
using HHTravel.CRM.Booking_Online.Entity;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;
using HHTravel.CRM.Booking_Online.DataAccess.TableWorkers;

namespace DataAccessTest
{
    [TestClass()]
    public class PropertyTableWorkerTest
    {
        PropertyTableWorker _repoProp = new PropertyTableWorker();

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
