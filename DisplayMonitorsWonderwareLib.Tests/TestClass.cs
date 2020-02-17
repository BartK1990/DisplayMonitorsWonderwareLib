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
    }
}