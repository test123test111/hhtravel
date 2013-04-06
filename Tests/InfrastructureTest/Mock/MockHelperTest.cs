using System;
using System.Linq;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.Infrastructure.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InfrastructureTest.Mock
{
    [TestClass]
    public class MockHelperTest
    {
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

        [TestMethod]
        public void GetRandomItemTest()
        {
            var travelTypes = Enum.GetValues(typeof(TravelType)).Cast<TravelType>();

            for (int i = 0; i < 1000; i++)
            {
                var a = MockHelper.GetRandomItem<TravelType>(travelTypes);
                if (a == TravelType.TravelType3)
                {
                    var j = i;
                }
            }

            Assert.IsTrue(true);
        }
    }
}