using HHTravel.CRM.Booking_Online.DataAccess.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using HHTravel.CRM.Booking_Online.Model;

namespace DataAccessTest
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
        #endregion


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
                ChildNum = null,
                ContactFavorite = 1,
                ConvinientTime = 2,
                Days = 8,
                DepartureDate = departueDate,
                Email = "abc@hhtravel.com",
                FirstName = "巴马",
                FirstNameEn = "bama",
                IsStay = null,
                OrderId = 1,
                OrderItemList = new System.Collections.Generic.List<OrderItem> { 
                    // 酒店
                    new OrderItem{
                        AdultNum = 2,
                        AdultPrice = 15000,
                        Amount = 30000,
                        ChildNum = null,
                        ChildPrice = null,
                        DepartureDate = departueDate.AddDays(1),    // 假设第二天入住
                        HotelDays = (short)(travel.Days - 1), // 假设比行程少一天
                        IsHotelStay = null, // 非单品游酒店，不提供延住服务
                        ProductId = 20, // todo: 找一个真实酒店的id   // todo: 选定酒店需是Travel的Resource
                        ReturnDate = null,
                        SpecId = 13,    // todo: 找一个规格，属于上述酒店的
                    },
                    // 机票
                    new OrderItem{
                        AdultNum = 2,
                        AdultPrice = null, // todo: 非单品游，航班没有单独定价
                        Amount = null,
                        ChildNum = null,
                        ChildPrice = null,
                        DepartureDate = departueDate,
                        ProductId = 21, // todo: 找一个真是航班的Id
                        SpecId = 14,    // todo: 找一个规格，属于上述航班的
                    }
                },
                PhoneNumber = "086010",
                RequestFrom = "www.hhtravel.com",
                ReturnDate = departueDate.AddDays(travel.Days - 1),
                SpecialHope = "给免单吧",
                Suranme = "奥",
                SurnameEn = "Ao",

                Product = travel,
            };
            Order expected = order;
            Order actual;
            actual = target.Insert(order);

            Assert.IsTrue(actual.OrderId > 0);
            Assert.AreEqual(expected, actual);
        }

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
    }
}
