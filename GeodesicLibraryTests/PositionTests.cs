using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Marknotgeorge.GeodesicLibrary;
using UnitsNet.Units;
using UnitsNet;


namespace GeodesicLibraryTests
{
    /// <summary>
    /// Summary description for PositionTests
    /// </summary>
    [TestClass]
    public class PositionTests
    {
        public PositionTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
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
        #endregion

        [TestMethod]
        public void GetDistanceToTest()
        {
            double expectedValue = 968.9;
            
            Position fromPosition = new Position(50.066389, -5.714722);
            Position toPosition = new Position(58.643889, -3.07);

            double testValue = fromPosition.DistanceTo(toPosition);

            double roundedDistance = Math.Round(testValue, 1);

            Assert.AreEqual(expectedValue, roundedDistance);

        }
    }
}
