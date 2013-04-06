using EFCachingProvider;
using EFCachingProvider.Caching;
using HHTravel.CRM.Booking_Online.Infrastructure.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InfrastructureTest.Caching
{
    /// <summary>
    /// Summary description for EFCachingPolicyTest
    /// </summary>
    [TestClass]
    public class EFCachingPolicyTest
    {
        private TestContext testContextInstance;

        public EFCachingPolicyTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            EFCachingProviderConfiguration.DefaultCache = new InMemoryCache();
        }

        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion Additional test attributes

        [TestMethod]
        public void InstancedPolicyTest()
        {
            EFCachingProviderConfiguration.DefaultCachingPolicy = new EFCachingPolicy();
            EFCachingProviderConfiguration.DefaultCache = new InMemoryCache();
        }
    }
}