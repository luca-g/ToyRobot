using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToyRobot.Common.Services;
using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.MockHelper;
using System.Threading.Tasks;

namespace ToyRobot.Core.Services.Commands.Tests
{
    [TestClass()]
    public class CreateRobotCommandServiceTests
    {
        [TestMethod()]
        [DataRow("CREATEROBOT", true)]
        [DataRow("CREATEROBOT 1", false)]
        [DataRow("CREATEROBO", false)]
        public void TryParseTest(string command, bool parsed)
        {
            var mock = new MockServicesHelper<CreateRobotCommandService>();
            var createRobotCommand = new CreateRobotCommandService(
                mock.Logger.Object,
                mock.MapService.Object,
                mock.RobotService.Object,
                mock.PlayerService.Object,
                mock.ApplicationMessageService);

            var parts = command.Split(new char[] { ' ', ',' });
            Assert.AreEqual(parsed, createRobotCommand.TryParse(parts));
        }

        [TestMethod()]
        [DataRow("CREATEROBOT")]
        public async Task ExecuteTest(string command)
        {          
            var mock = new MockServicesHelper<CreateRobotCommandService>();
            mock.ActiveMapSetupProperty(1)
                .ActivePlayerSetupProperty(1)
                .ActiveRobotSetupProperty(null)
                .CreateRobotSetup();
            var createRobotCommand = new CreateRobotCommandService(
                mock.Logger.Object,
                mock.MapService.Object,
                mock.RobotService.Object,
                mock.PlayerService.Object,
                mock.ApplicationMessageService);

            var parts = command.Split(new char[] { ' ', ',' });
            Assert.AreEqual(true, createRobotCommand.TryParse(parts));

            var result = await createRobotCommand.Execute();

            Assert.AreEqual(true, result);
            Assert.AreEqual(createRobotCommand.CommandResult, CommandResultEnum.Ok);
        }

        [TestMethod()]
        [DataRow("CREATEROBOT")]
        public async Task ExecuteTest_NoMapSelected(string command)
        {
            var mock = new MockServicesHelper<CreateRobotCommandService>();
            mock.ActivePlayerSetupProperty(1)
                .CreateRobotSetup();
            var createRobotCommand = new CreateRobotCommandService(
                mock.Logger.Object,
                mock.MapService.Object,
                mock.RobotService.Object,
                mock.PlayerService.Object,
                mock.ApplicationMessageService);

            var parts = command.Split(new char[] { ' ', ',' });
            Assert.AreEqual(true, createRobotCommand.TryParse(parts));

            var result = await createRobotCommand.Execute();

            Assert.AreEqual(false, result);
            Assert.AreEqual(createRobotCommand.CommandResult, CommandResultEnum.ActiveMapNull);
        }
        [TestMethod()]
        [DataRow("CREATEROBOT")]
        public async Task ExecuteTest_CreateRobotError(string command)
        {
            var mock = new MockServicesHelper<CreateRobotCommandService>();
            mock.ActiveMapSetupProperty(1)
                .ActivePlayerSetupProperty(1)
                .ActiveRobotSetupProperty(null);
            var createRobotCommand = new CreateRobotCommandService(
                mock.Logger.Object,
                mock.MapService.Object,
                mock.RobotService.Object,
                mock.PlayerService.Object,
                mock.ApplicationMessageService);

            var parts = command.Split(new char[] { ' ', ',' });
            Assert.AreEqual(true, createRobotCommand.TryParse(parts));

            var result = await createRobotCommand.Execute();

            Assert.AreEqual(false, result);
            Assert.AreEqual(createRobotCommand.CommandResult, CommandResultEnum.CreateRobotFailed);
        }

    }
}