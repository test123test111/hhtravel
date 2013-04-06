using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.Providers;
using HHTravel.CRM.Booking_Online.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessTest
{
    /// <summary>
    ///This is a test class for CustomersProviderTest and is intended
    ///to contain all CustomersProviderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CustomersProviderTest
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
            CustomersProvider target = new CustomersProvider();
            int id = 0;
            Customer expected = null;
            Customer actual;
            actual = target.Find(id);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void InsertTest()
        {
            CustomersProvider target = new CustomersProvider();
            var customer = new Customer { };
            var actual = target.Insert(customer);

            Assert.AreEqual(customer, actual);
        }

        [TestMethod()]
        public void MarketingHtmlTest()
        {
            CustomerDbEntities cxt = new CustomerDbEntities(
        DbContextFactory.GetEntityConnectionString("Data Source=hhtravel.db.sh.ctriptravel.com,55944;Initial Catalog=CustomerDB;Persist Security Info=True;User ID=ws_hhtravel;Password=1qaz2wsx!QAZ@WSX",
        "CustomerDb"));
            var a = from p in cxt.Mkt_Html
                    select p;
            var b = a.ToList();
        }
    }
}