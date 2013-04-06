﻿using HHTravel.CRM.Booking_Online.DataAccess.Providers;
using HHTravel.CRM.Booking_Online.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessTest
{
    /// <summary>
    ///This is a test class for OrdersProviderTest and is intended
    ///to contain all OrdersProviderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OrdersProviderTest
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

        /// <summary>
        ///A test for Find
        ///</summary>
        [TestMethod()]
        public void FindTest()
        {
            OrdersProvider target = new OrdersProvider();
            int id = 0; // TODO: Initialize to an appropriate value
            Orders expected = null; // TODO: Initialize to an appropriate value
            Orders actual;
            actual = target.Find(id);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void InsertTest()
        {
            OrdersProvider target = new OrdersProvider();
            var order = new Orders { };
            var actual = target.Insert(order);

            Assert.AreEqual(order, actual);
        }
    }
}