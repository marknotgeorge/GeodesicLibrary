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

        private double delta = 0.1; 

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

            

            Assert.AreEqual(expectedValue, testValue, delta);

        }

        [TestMethod]
        public void InitialBearingTest()
        {
            double expectedValue = 9.119722;

            Position fromPosition = new Position(50.066389, -5.714722);
            Position toPosition = new Position(58.643889, -3.07);

            double testValue = fromPosition.InitialBearing(toPosition);

            Assert.AreEqual(expectedValue, testValue, delta);

        }

        [TestMethod]
        public void FinalBearingTest()
        {
            double expectedValue = 11.275278;

            Position fromPosition = new Position(50.066389, -5.714722);
            Position toPosition = new Position(58.643889, -3.07);

            double testValue = fromPosition.FinalBearing(toPosition);
            

            Assert.AreEqual(expectedValue, testValue, delta);

        }

        [TestMethod]
        public void MidpointTest()
        {
            double expectedLat = 54.362222;
            double expectedLon = -4.530556;

            Position fromPosition = new Position(50.066389, -5.714722);
            Position toPosition = new Position(58.643889, -3.07);

            Position testValue = fromPosition.MidpointTo(toPosition);

            Assert.AreEqual(expectedLat, testValue.Latitude, delta);
            Assert.AreEqual(expectedLon, testValue.Longitude, delta);
        }

        [TestMethod]
        public void DestinationTest()
        {
            double expectedLat = 53.188333;
            double expectedLon = 0.133333;

            Position fromPosition = new Position(53.320556, -1.729722);
            double bearing = 96.021667;
            double distance = 124.8; // km

            Position testValue  = fromPosition.Destination(bearing, distance);           

            Assert.AreEqual(expectedLat, testValue.Latitude, delta);
            Assert.AreEqual(expectedLon, testValue.Longitude, delta);
        }

        [TestMethod]
        public void IntersectionTest()
        {
            double expectedLat = 50.901667;
            double expectedLon = 4.494167;

            Position firstPosition = new Position(51.885, 0.235);
            Position secondPosition = new Position(49.008, 2.549);
            double firstBearing = 108.63;
            double secondBearing = 32.72;

            Position testValue = firstPosition.Intersection(firstBearing, secondPosition, secondBearing);

            Assert.AreEqual(expectedLat, testValue.Latitude, delta);
            Assert.AreEqual(expectedLon, testValue.Longitude, delta);
        }
    }
}
