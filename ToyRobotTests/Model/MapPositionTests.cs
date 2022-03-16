using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ToyRobot.Model.Tests;

[TestClass()]
public class MapPositionTests
{
    [TestMethod()]
    public void ToStringTest()
    {
        var mapPositon = new MapPosition(0, 0, MapOrientationEnum.NORTH);
        Assert.AreEqual("0,0,NORTH", mapPositon.ToString());
    }

    [TestMethod()]
    public void LeftTest()
    {
        var mapPositon = new MapPosition(1, 0, MapOrientationEnum.NORTH);
        mapPositon.Left();
        var s = mapPositon.ToString();
        Assert.AreEqual(s, "1,0,WEST");
        s = mapPositon.Move().ToString();
        Assert.AreEqual(s, "0,0,WEST");
    }

    [TestMethod()]
    public void RightTest()
    {
        var mapPositon = new MapPosition(0, 1, MapOrientationEnum.EAST);
        mapPositon.Right();
        var s = mapPositon.ToString();
        Assert.AreEqual(s, "0,1,SOUTH");
        s = mapPositon.Move().ToString();
        Assert.AreEqual(s, "0,0,SOUTH");
    }

    [TestMethod()]
    public void MoveTest()
    {
        var mapPositon = new MapPosition(0, 0, MapOrientationEnum.NORTH);
        var s = mapPositon.Move().ToString();
        Assert.AreEqual(s, "0,1,NORTH");
    }
}
