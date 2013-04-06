using System;
using HHTravel.CRM.Booking_Online.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InfrastructureTest
{
    /// <summary>
    ///This is a test class for GlobalConfigTest and is intended
    ///to contain all GlobalConfigTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GlobalConfigTest
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
        ///A test for GetFromAppSettings
        ///</summary>
        [TestMethod]
        public void GetFromAppSettingsTest()
        {
            string key = "ContainsProductsNotUp";
            bool expected = true;
            bool actual;
            actual = GlobalConfig.GetFromAppSettings<bool>(key, true);
            Assert.AreEqual(expected, actual);

            expected = true;
            actual = GlobalConfig.GetFromAppSettings<bool>(key, false);
            Assert.AreEqual(expected, actual);

            key = "ContainsProductsNotUp2";
            expected = true;
            actual = GlobalConfig.GetFromAppSettings<bool>(key, false);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetNullableTest()
        {
            bool? actual;
            bool? expected = false;
            actual = GlobalConfig.ConvertToNullable<bool>(false);
            Assert.AreEqual<bool?>(expected, actual);

            expected = null;
            actual = GlobalConfig.ConvertToNullable<bool>(null);
            Assert.AreEqual<bool?>(expected, actual);

            expected = false;
            actual = GlobalConfig.ConvertToNullable<bool>(0);
            Assert.AreEqual<bool?>(expected, actual);

            expected = true;
            actual = GlobalConfig.ConvertToNullable<bool>(1);
            Assert.AreEqual<bool?>(expected, actual);

            expected = true;
            actual = GlobalConfig.ConvertToNullable<bool>("true");
            Assert.AreEqual<bool?>(expected, actual);

            expected = true;
            actual = GlobalConfig.ConvertToNullable<bool>("True");
            Assert.AreEqual<bool?>(expected, actual);

            expected = true;
            actual = GlobalConfig.ConvertToNullable<bool>("TrUE");
            Assert.AreEqual<bool?>(expected, actual);

            expected = true;
            actual = GlobalConfig.ConvertToNullable<bool>("TRUE");
            Assert.AreEqual<bool?>(expected, actual);

            expected = null;
            actual = GlobalConfig.ConvertToNullable<bool>("");
            Assert.AreEqual<bool?>(expected, actual);

            expected = null;
            actual = GlobalConfig.ConvertToNullable<bool>(DateTime.Now);
            Assert.AreEqual<bool?>(expected, actual);
        }
    }
}