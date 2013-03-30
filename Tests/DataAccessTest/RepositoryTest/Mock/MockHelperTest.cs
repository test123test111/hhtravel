using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HHTravel.CRM.Booking_Online.Model;
using HHTravel.CRM.Booking_Online.DataAccess.Mock;

namespace DataAccessTest.RepositoryTest.Mock
{
    [TestClass]
    public class MockHelperTest
    {
        [TestMethod]
        public void GetRandomItemTest()
        {
            var travelTypes = Enum.GetValues(typeof(TravelType)).Cast<TravelType>();

            for(int i = 0; i < 1000; i++)
            {
                var a = MockHelper.GetRandomItem<TravelType>(travelTypes);
               if (a == TravelType.单品游)
               {
                   var j = i;
               }
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void FooTest()
        {
            var travelTypes = Enum.GetValues(typeof(TravelType)).Cast<TravelType>();

            var a = from tt in travelTypes
                    where false && false
                    select tt;

            Assert.IsTrue(a.Count() == 0);

            var b = from tt in travelTypes
                    where false && true
                    select tt;

            Assert.IsTrue(b.Count() == 0);

            var c = from tt in travelTypes
                    where true && true
                    select tt;

            Assert.IsTrue(c.Count() > 0);

            var d = from tt in travelTypes
                    where false && false && true
                    select tt;

            Assert.IsTrue(d.Count() == 0);
        }
    }
}
