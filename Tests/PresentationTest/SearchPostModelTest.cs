using System;
using HHTravel.CRM.Booking_Online.Site.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace PresentationTest
{
    /// <summary>
    ///This is a test class for SearchPostModelTest and is intended
    ///to contain all SearchPostModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SearchPostModelTest
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
        ///A test for ProductName
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]

        //[HostType("ASP.NET")]
        //[UrlToTest("http://local.hhtravel.com")]
        public void ProductNameTest()
        {
            SearchPostModel target = new SearchPostModel();
            string expected = "伦敦 + 巴黎专属车导TravelType2 9 天";
            string actual;
            target.ProductName = "伦敦 + 巴黎专属车导TravelType2 9 天（上海出发）";
            actual = target.ProductName;
            Assert.AreEqual(expected, actual);
        }
    }
}