using System;
using HHTravel.CRM.Booking_Online.Site.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PresentationTest
{
    /// <summary>
    ///This is a test class for CalendarModelTest and is intended
    ///to contain all CalendarModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CalendarModelTest
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
        ///A test for CalendarModel Constructor
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]

        //[HostType("ASP.NET")]
        //[UrlToTest("http://local.hhtravel.com")]
        public void CalendarModelConstructorTest1()
        {
            DateTime beginDate = new DateTime(2012, 11, 1);
            DateTime endDate = new DateTime(2014, 11, 1);
            CalendarModel target;

            target = new CalendarModel(beginDate, endDate, 2012, 10);
            Assert.IsTrue(target.MonthModel == null);

            target = new CalendarModel(beginDate, endDate, 2012, 11);
            Assert.IsTrue(target.MonthModel != null);

            beginDate = new DateTime(2012, 10, 1);
            endDate = new DateTime(2014, 10, 1);
            target = new CalendarModel(beginDate, endDate, 2012, 11);
            Assert.IsTrue(target.BeginMonth == 11);
            Assert.IsTrue(target.MonthModel != null);
        }
    }
}