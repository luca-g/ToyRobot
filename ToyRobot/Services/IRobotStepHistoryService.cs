using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyRobot.Model;

namespace ToyRobot.Services
{
    public interface IRobotStepHistoryService
    {
        void AddResizeMapStep(int width, int height);
        void AddStep(
            MapPosition? positionBeforeCommand,
            MapPosition? positionAfterCommand,
            string command,
            bool commandExecuted,
            string? result
            );
    }
    public class DummyRobotStepHistoryService : IRobotStepHistoryService
    {
        public void AddResizeMapStep(int width, int height)
        { }
        public void AddStep(
            MapPosition? positionBeforeCommand,
            MapPosition? positionAfterCommand,
            string command,
            bool commandExecuted,
            string? result
            )
        { }
    }
}
