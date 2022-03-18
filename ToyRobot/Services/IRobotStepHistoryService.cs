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
        Task AddResizeMapStepAsync(int width, int height);
        Task AddStep(
            MapPosition? positionBeforeCommand,
            MapPosition? positionAfterCommand,
            string command,
            bool commandExecuted,
            string? result
            );
    }
    public class DummyRobotStepHistoryService : IRobotStepHistoryService
    {
        public async Task AddResizeMapStepAsync(int width, int height)
        { await Task.CompletedTask; }
        public async Task AddStep(
            MapPosition? positionBeforeCommand,
            MapPosition? positionAfterCommand,
            string command,
            bool commandExecuted,
            string? result
            )
        { await Task.CompletedTask; }
    }
}
