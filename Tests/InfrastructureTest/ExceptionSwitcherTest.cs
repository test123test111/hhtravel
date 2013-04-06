using System;
using HHTravel.CRM.Booking_Online.Infrastructure;
using HHTravel.CRM.Booking_Online.Infrastructure.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InfrastructureTest
{
    /// <summary>
    ///This is a test class for ExceptionSwitcherTest and is intended
    ///to contain all ExceptionSwitcherTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExceptionSwitcherTest
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

        [TestMethod()]
        public void TryThrowTest()
        {
            try
            {
                ExceptionSwitcher<Exception>.TryThrow(false, new Exception());
            }
            catch (Exception)
            {
                Assert.Inconclusive("要求不抛出异常");
            }

            try
            {
                ExceptionSwitcher<Exception>.TryThrow(true, new Exception());
            }
            catch (Exception)
            {
                Assert.IsTrue(true, "要求抛出异常");
            }

            var expected = new DataInvalidException();
            try
            {
                ExceptionSwitcher<Exception>.TryThrow(true, expected);
            }
            catch (Exception e)
            {
                Assert.IsTrue(true, "要求抛出异常");
                Assert.AreEqual<DataInvalidException>(expected, e as DataInvalidException);
            }
        }
    }
}