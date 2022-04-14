using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToyRobot.API.Controllers;
using ToyRobot.API.Model;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.API.Controllers.Tests;

[TestClass()]
public class UserControllerTests
{
    [TestMethod()]
    public async Task LoginTest()
    {
        Mock<IPlayer> player = new();
        Mock<IPlayerService> playerService = new();
        playerService.Setup(t => t.LoadPlayer(It.IsAny<System.Guid>()))
            .ReturnsAsync(player.Object);

        var controller = new UserController(playerService.Object);
        var model = new LoginModel();
        var result = await controller.Login(model);
        if(result is not OkObjectResult okResult)
        {
            Assert.Fail();
            return;
        }
        Assert.IsTrue(okResult.StatusCode == 200);
        Assert.IsNotNull(okResult.Value);
        Assert.IsInstanceOfType(okResult.Value, typeof(IPlayer));
    }

    [TestMethod()]
    public async Task LoginTest_UserNotFound()
    {
        Mock<IPlayer> player = new();
        Mock<IPlayerService> playerService = new();
        playerService.Setup(t => t.LoadPlayer(It.IsAny<System.Guid>()))
            .ThrowsAsync(new KeyNotFoundException());

        var controller = new UserController(playerService.Object);
        var model = new LoginModel();
        var result = await controller.Login(model);
        if (result is not NotFoundResult notFoundResult)
        {
            Assert.Fail();
            return;
        }
        Assert.IsTrue(notFoundResult.StatusCode == StatusCodes.Status404NotFound);
    }
    [TestMethod()]
    public async Task CreateTest()
    {
        Mock<IPlayer> player = new();
        Mock<IPlayerService> playerService = new();
        playerService.Setup(t => t.CreatePlayer())
            .ReturnsAsync(player.Object);

        var controller = new UserController(playerService.Object);
        var model = new LoginModel();
        var result = await controller.Create();
        if (result is not OkObjectResult okResult)
        {
            Assert.Fail();
            return;
        }
        Assert.IsTrue(okResult.StatusCode == 200);
        Assert.IsNotNull(okResult.Value);
        Assert.IsInstanceOfType(okResult.Value, typeof(IPlayer));
    }
    [TestMethod()]
    public async Task CreateTest_Failed()
    {
        Mock<IPlayer> player = new();
        Mock<IPlayerService> playerService = new();
        playerService.Setup(t => t.CreatePlayer())
            .ThrowsAsync(new System.Exception());

        var controller = new UserController(playerService.Object);
        var result = await controller.Create();
        if (result is not StatusCodeResult callResult)
        {
            Assert.Fail();
            return;
        }
        Assert.IsTrue(callResult.StatusCode == StatusCodes.Status500InternalServerError);
    }
}
