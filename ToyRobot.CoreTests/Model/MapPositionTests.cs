using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToyRobot.Common.Model;

namespace ToyRobot.Core.Model.Tests;

[TestClass()]
public class MapPositionTests
{
    [TestMethod()]
    [DataRow(1, 2, MapOrientationEnum.NORTH, "1,2,NORTH")]
    [DataRow(1, 2, MapOrientationEnum.SOUTH, "1,2,SOUTH")]
    [DataRow(1, 2, MapOrientationEnum.EAST, "1,2,EAST")]
    [DataRow(1, 2, MapOrientationEnum.WEST, "1,2,WEST")]
    public void ToStringTest(int x, int y, MapOrientationEnum mapOrientationEnum, string expected)
    {
        var mapPositon = new MapPosition(x, y, mapOrientationEnum);
        Assert.AreEqual(expected, mapPositon.ToString());
    }

    [TestMethod()]
    [DataRow(1, 1, MapOrientationEnum.NORTH, MapOrientationEnum.WEST)]
    [DataRow(1, 1, MapOrientationEnum.SOUTH, MapOrientationEnum.EAST)]
    [DataRow(1, 1, MapOrientationEnum.EAST, MapOrientationEnum.NORTH)]
    [DataRow(1, 1, MapOrientationEnum.WEST, MapOrientationEnum.SOUTH)]
    public void LeftTest(int x, int y, MapOrientationEnum mapOrientationEnum, MapOrientationEnum expected)
    {
        var mapPosition = new MapPosition(x, y, mapOrientationEnum);
        var newPosition = mapPosition.Left();
        Assert.AreEqual(newPosition.Orientation, expected);
    }

    [TestMethod()]
    [DataRow(1, 1, MapOrientationEnum.NORTH, MapOrientationEnum.EAST)]
    [DataRow(1, 1, MapOrientationEnum.SOUTH, MapOrientationEnum.WEST)]
    [DataRow(1, 1, MapOrientationEnum.EAST, MapOrientationEnum.SOUTH)]
    [DataRow(1, 1, MapOrientationEnum.WEST, MapOrientationEnum.NORTH)]
    public void RightTest(int x, int y, MapOrientationEnum mapOrientationEnum, MapOrientationEnum expected)
    {
        var mapPosition = new MapPosition(x, y, mapOrientationEnum);
        var newPosition = mapPosition.Right();
        Assert.AreEqual(newPosition.Orientation, expected);
    }

    [TestMethod()]
    [DataRow(1, 1, MapOrientationEnum.NORTH, 1, 2)]
    [DataRow(1, 1, MapOrientationEnum.SOUTH, 1, 0)]
    [DataRow(1, 1, MapOrientationEnum.EAST, 2, 1)]
    [DataRow(1, 1, MapOrientationEnum.WEST, 0, 1)]
    public void MoveTest(int x, int y, MapOrientationEnum mapOrientationEnum, int expectedX, int expectedY)
    {
        var mapPosition = new MapPosition(x, y, mapOrientationEnum);
        var newMapPosition = mapPosition.Move();
        Assert.AreEqual(expectedX, newMapPosition.X);
        Assert.AreEqual(expectedY, newMapPosition.Y);
        Assert.AreEqual(x, mapPosition.X);
        Assert.AreEqual(y, mapPosition.Y);
    }
}
