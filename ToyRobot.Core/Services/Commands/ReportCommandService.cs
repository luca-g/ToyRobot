﻿using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Services.Commands;

public class ReportCommandService : ICommand
{
    public ICommandText CommandInstructions { get; private set; }
    private readonly ILogger<ReportCommandService> loggerService;
    private readonly IApplicationMessagesService applicationMessagesService;
    public string? ExecuteResultText { get; set; }
    public CommandResultEnum CommandResult { get; set; }

    public ReportCommandService(
        ICoreFactoryService coreFactoryService,
        ILogger<ReportCommandService> logger,
        IApplicationMessagesService applicationMessagesService)
    {
        this.loggerService = logger;
        this.applicationMessagesService = applicationMessagesService;
        this.CommandInstructions =
            coreFactoryService.CreateCommandInstructionsBuilder()
            .SetFirstInstruction("REPORT")
            .Build();
    }
    public Task<bool> Execute(IScenario scenario)
    {
        try
        {
            if (!scenario.IsRobotSet)
            {
                this.loggerService.LogTrace("REPORT command: active robot is null");
                applicationMessagesService.SetResult(scenario.Language, this, CommandResultEnum.ActiveRobotNull);
                return Task.FromResult(false);
            }
            if (!scenario.IsRobotDeployed)
            {
                applicationMessagesService.SetResult(scenario.Language, this, CommandResultEnum.RobotPositionNull);
                this.loggerService.LogTrace("REPORT command result: {ExecuteResult}", this.ExecuteResultText);
            }
            else
            {
                this.ExecuteResultText = scenario.RobotPosition?.ToString() ?? "position not set";
                this.CommandResult = CommandResultEnum.Ok;
                this.loggerService.LogTrace("REPORT command result: {ExecuteResult}", this.ExecuteResultText);
            }
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            this.loggerService.LogError(ex, "REPORT Command unexpected error");
            return Task.FromException<bool>(ex);
        }
    }

    public bool TryParse(string[] commandParts)
    {
        if (commandParts.Length != 1)
            return false;
        return CommandInstructions.CommandName.Equals(commandParts[0]);
    }
}

