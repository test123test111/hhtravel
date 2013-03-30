using HHTravel.CRM.Booking_Online.DataAccess.TableWorkers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.Entity;

namespace DataAccessTest
{
    
    
    /// <summary>
    ///This is a test class for OrdersTableWorkerTest and is intended
    ///to contain all OrdersTableWorkerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OrdersProductTableWorkerTest
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
        #endregion


        [TestMethod()]
        public void InsertTest()
        {
            OrdersProductTableWorker target = new OrdersProductTableWorker();
            var ordersProduct = new Orders_Product { 
            };
            var actual = target.Insert(ordersProduct);

            Assert.AreEqual(ordersProduct, actual);
        }

        /// <summary>
        ///A test for Find
        ///</summary>
        [TestMethod()]
        public void FindTest()
        {
            OrdersTableWorker target = new OrdersTableWorker();
            int id = 0; // TODO: Initialize to an appropriate value
            Orders expected = null; // TODO: Initialize to an appropriate value
            Orders actual;
            actual = target.Find(id);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
