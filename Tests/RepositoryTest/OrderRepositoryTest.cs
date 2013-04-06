using System;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RepositoryTest
{
    /// <summary>
    ///This is a test class for OrderRepositoryTest and is intended
    ///to contain all OrderRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OrderRepositoryTest : RepositoryTestBase
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
            OrderRepository target = new OrderRepository(); // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            Order expected = null; // TODO: Initialize to an appropriate value
            Order actual;
            actual = target.Find(id);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Insert
        ///</summary>
        [TestMethod()]
        public void InsertTest()
        {
            OrderRepository target = new OrderRepository();

            var departueDate = new DateTime(2012, 12, 22);
            Product travel = new ProductRepository().Find(9);
            Order order = new Order
            {
                AdultNum = 2,
                AirTicketOneself = null,
                ChildNum = 0,
                ContactFavorite = (ContactFavorite)1,
                ConvinientTime = (ConvinientTime)2,
                Days = 8,
                DepartDate = departueDate,
                Email = "abc@hhtravel.com",
                FirstName = "奥",
                FirstNameEn = "bama",
                IsHotelStay = false,
                OrderId = 1,
                OrderItemList = new System.Collections.Generic.List<OrderItem> {

                    // 酒店
                    new OrderItem("酒店", 20, "", 13, "", 2, 1500, departueDate.AddDays(1), null, "间"),

                    // 机票
                    new OrderItem("机票", 21, "", 14, "", 2, 0, departueDate, null, "间"),
                },
                PhoneNumber = "086010",
                RequestFrom = "www.hhtravel.com",
                PlanReturnDate = departueDate.AddDays(travel.Days - 1),
                SpecialHope = "给免单吧",
                LastName = "巴马",
                LastNameEn = "Ao",

                Product = travel,
            };
            Order expected = order;
            Order actual;
            actual = target.Add(order);

            Assert.IsTrue(actual.OrderId > 0);
            Assert.AreEqual(expected, actual);
        }
    }
}