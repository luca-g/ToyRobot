using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.APITests;
using ToyRobot.Common.Services;
using ToyRobot.Core.Model;
using ToyRobot.Common.Model;
using ToyRobot.API.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ToyRobot.API.Controllers.Tests
{
    [TestClass()]
    public class CommandControllerTests
    {
        private static async Task<(IScenario, CommandController)> GetCommandController()
        {
            var mock = new MockServicesHelper<CommandController>();
            var host = mock.GetHost();
            var commandCenter = (ICommandCenterService?)host.Services.GetService(typeof(ICommandCenterService));
            var factoryService = (IFactoryService?)host.Services.GetService(typeof(IFactoryService));
            var playerService = (IPlayerService?)host.Services.GetService(typeof(IPlayerService));

            Assert.IsNotNull(factoryService);
            Assert.IsNotNull(commandCenter);
            Assert.IsNotNull(playerService);
            var player = await playerService.CreatePlayer();
            Assert.IsNotNull(player);
            var scenario = await factoryService.CreateScenario(player.PlayerGuid, null, null);
            Assert.IsNotNull(scenario);
            mock.SetUserGuid(player.PlayerGuid.ToString());

            var controller = new CommandController(
                mock.Logger.Object,
                commandCenter,
                factoryService,
                mock.HttpContextAccessor.Object);
            return (scenario, controller);
        }
        [TestMethod()]
        public async Task GetTest()
        {
            var (_, controller) = await GetCommandController();
            var result = controller.Get();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);
            if (result.Result is not OkObjectResult okResult)
            {
                Assert.Fail();
                return;
            }
            var items = okResult.Value as IEnumerable<ICommandText>;
            Assert.IsNotNull(items);
            Assert.IsTrue(items.Any());
        }
        [TestMethod()]
        [DataRow("PLACE 1,1,NORTH")]
        [DataRow("MOVE")]
        public async Task CommandTest(string command_text)
        {
            var (scenario, controller) = await GetCommandController();
            var executeCommandModel = new ExecuteCommandModel
            {
                MapId = scenario.MapId,
                RobotId = scenario.RobotId,
                Text = command_text
            };
            var result = await controller.Command(executeCommandModel);
            if (result.Result is not OkObjectResult okResult)
            {
                Assert.Fail();
                return;
            }
            Assert.IsTrue(okResult.StatusCode == 200);
            Assert.IsNotNull(okResult.Value);
            Assert.IsInstanceOfType(okResult.Value, typeof(ExecuteCommandModel));
        }
        [TestMethod()]
        public async Task CommandsIntegrationTest()
        {
            var mock = new MockServicesHelper<CommandController>();
            var host = mock.GetHost();
            var commandCenter = (ICommandCenterService?)host.Services.GetService(typeof(ICommandCenterService));
            var factoryService = (IFactoryService?)host.Services.GetService(typeof(IFactoryService));

            Assert.IsNotNull(factoryService);
            Assert.IsNotNull(commandCenter);
            var scenario = await factoryService.CreateScenario();
            Assert.IsNotNull(scenario);

            Assert.IsTrue(await commandCenter.Execute(scenario, "PLACE 1,1,NORTH"));
            IMapPosition? position = new MapPosition(1, 1, Common.Model.MapOrientationEnum.NORTH);
            Assert.AreEqual(position.ToString(), scenario.RobotPosition?.ToString());

            Assert.IsTrue(await commandCenter.Execute(scenario, "LEFT"));
            position = new MapPosition(1, 1, Common.Model.MapOrientationEnum.WEST);
            Assert.AreEqual(position.ToString(), scenario.RobotPosition?.ToString());

            Assert.IsTrue(await commandCenter.Execute(scenario, "MOVE"));
            position = new MapPosition(0, 1, Common.Model.MapOrientationEnum.WEST);
            Assert.AreEqual(position.ToString(), scenario.RobotPosition?.ToString());

            Assert.IsTrue(await commandCenter.Execute(scenario, "MOVE"));
            Assert.AreEqual(position.ToString(), scenario.RobotPosition?.ToString());
        }
    }
}