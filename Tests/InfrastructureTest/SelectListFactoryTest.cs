using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HHTravel.CRM.Booking_Online.Infrastructure.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InfrastructureTest
{
    /// <summary>
    ///This is a test class for SelectListFactoryTest and is intended
    ///to contain all SelectListFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SelectListFactoryTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion Additional test attributes

        [TestMethod()]
        public void CreateAddAllTest()
        {
            SelectListItem actual;
            SelectListItem expected = new SelectListItem
            {
                Text = "全部",
                Value = "",
            };
            var sl = SelectListFactory.Create<TravelTypeInt, int>(true);
            actual = sl.First();
            Assert.AreEqual(expected.Text, actual.Text);
            Assert.AreEqual(expected.Value, actual.Value);
            Assert.AreEqual(expected.Selected, actual.Selected);
        }

        [TestMethod()]
        public void CreateTest()
        {
            CreateTestHelper<TravelTypeDefault, int>();
            CreateTestHelper<TravelTypeDefault, long>();
            CreateTestHelper<TravelTypeDefault, short>();

            CreateTestHelper<TravelTypeInt, int>();
            CreateTestHelper<TravelTypeInt, long>();

            CreateTestHelper<TravelTypeLong, long>();
        }

        #region TestHelper

        public enum TravelTypeDefault
        {
            TravelType3,
            TravelType2,
            团队游,
        }

        public enum TravelTypeInt : int
        {
            TravelType3 = 3,
            TravelType2 = 1,
            团队游 = 2,
        }

        public enum TravelTypeLong : long
        {
            TravelType3 = 3000000000L,
            TravelType2 = 1000000000L,
            团队游 = 2000000000L,
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        public void CreateTestHelper<TEnum, T>()
            where TEnum : struct
            where T : struct
        {
            Type actual;
            bool addAll = false;
            IEnumerable<TEnum> collection = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            Type expected = typeof(T);
            var sl = SelectListFactory.Create<TEnum, T>(collection, addAll);
            actual = Convert.ChangeType(sl.First().Value, typeof(T)).GetType();
            Assert.AreEqual(expected, actual);
        }

        #endregion TestHelper
    }
}