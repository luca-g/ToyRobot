using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToyRobot.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRobot.Core.Configuration.Tests
{
    [TestClass()]
    public class MapSettingsTests
    {
        [TestMethod()]
        [DataRow(5, 5, 5, 5)]
        [DataRow(5, 4, 5, 5)]
        [DataRow(4, 5, 5, 5)]
        [DataRow(140, 5, 100, 5)]
        [DataRow(100, 5, 100, 5)]
        [DataRow(5, 101, 5, 100)]
        public void GetValidSizeTest(int width, int height, int expectedWidth, int expectedHeight)
        {
            var mapSettings = new MapSettings() {
                MinWidth = 5, MaxWidth = 100, MinHeight = 5, MaxHeight = 100 };
            Assert.AreEqual((expectedWidth, expectedHeight), mapSettings.GetValidSize(width, height)); ;
        }
    }
}