using System;
using EFCachingProvider;
using EFCachingProvider.Caching;
using HHTravel.CRM.Booking_Online.DataAccess;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessTest
{
    /// <summary>
    ///This is a test class for DbContextFactoryTest and is intended
    ///to contain all DbContextFactoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DbContextFactoryTest
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //EFTracingProviderConfiguration.RegisterProvider();
            //EFCachingProviderConfiguration.RegisterProvider();

            EFCachingProviderConfiguration.DefaultCache = new InMemoryCache();
        }

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
        public void CreateTest()
        {
            CreateTestHelper<ProductDbEntities>();
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        public void CreateTestHelper<T>()
            where T : DbEntitiesBase
        {
            T actual;
            actual = DbContextFactory.Create<T>(false);
            Assert.IsNotNull(actual);

            try
            {
                // do something...
                throw new NotSupportedException();
            }
            catch (NotSupportedException)
            {
                Assert.Fail("Don't rethrow NotSupportException! ");
            }

            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void UrlCombineTest()
        {
            string uriString = "http://localhost/HHTravel.CRM.Booking-Online.Site/Product/List/1/44";
            string queryString = "pageIndex=1";
            Uri baseUri = new Uri(uriString);
            Uri newUri = new Uri(baseUri, queryString);
            var expected = string.Format("{0}?{1}", uriString, queryString);
            string actual = newUri.ToString();

            Assert.AreEqual(actual, expected);
        }
    }
}