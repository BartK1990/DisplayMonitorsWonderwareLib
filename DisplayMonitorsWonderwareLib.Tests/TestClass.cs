// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;

namespace DisplayMonitorsWonderwareLib.Tests
{
    [TestFixture]
    public class TestClass
    {
        private DisplayMonitors dm;

        [OneTimeSetUp]
        public void SetUpDisplayMonitors()
        {
            dm = new DisplayMonitors();
        }

        [Test]
        public void TestMonitorsCoordinates()
        {
            int[] cords = dm.MonitorsCoordinates();
            Assert.That(cords.Length, Is.GreaterThan(0), "Are you testing without displays?");
        }

        [Test]
        public void TestMonitorsCoordinates0Based()
        {
            int[] cords = dm.MonitorsCoordinates0Based();
            Assert.That(cords.Length, Is.GreaterThan(0), "Are you testing without displays?");
            foreach (var cord in cords)
            {
                Assert.That(cord, Is.GreaterThanOrEqualTo(0), $"There is coordinate lesser than 0 : {cord}");
            }
        }
        [Test]
        public void TestCoordinatesTopLeftFirst()
        {
            int[] cords = dm.MonitorsCoordinatesTopLeftFirst();
            Assert.That(cords.Length, Is.GreaterThan(0), "Are you testing without displays?");
            if (cords.Length > 3)
            {
                for (int i = 1; i < cords.Length/2; i++)
                {
                    TestContext.WriteLine($"Testing cords pair number {i}.Current X: {cords[i*2]}, Y: {cords[i*2+1]} with previous X: {cords[(i-1)*2]}, Y: {cords[(i-1)*2+1]}");
                    Assert.That(cords[i*2]+cords[i*2+1], Is.GreaterThanOrEqualTo(cords[(i-1)*2]+cords[(i-1)*2+1]), $"Coordinates are not sorted properly");
                }
            }
        }
    }
}