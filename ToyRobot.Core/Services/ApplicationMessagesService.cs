﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Services;

public class ApplicationMessagesService : IApplicationMessagesService
{
    const string CommandResultLanguage = "CommandResult.{0}";
    const string DefaultLanguage = "en";
    private readonly ILogger<ApplicationMessagesService> loggerService;
    private readonly IConfiguration configuration;
    private readonly IPlayerService playerService;
    public ApplicationMessagesService(
        ILogger<ApplicationMessagesService> logger,
        IConfiguration configuration,
        IPlayerService playerService
        )
    {
        this.loggerService = logger;
        this.configuration = configuration;
        this.playerService = playerService;
    }
    public IConfiguration LoadCommandResult(string language)
    {
        try
        {
            loggerService.LogTrace("Loading language file {language}",language);
            var sectionName = string.Format(CommandResultLanguage, language);
            var section = configuration.GetSection(sectionName);
            if (section == null && !DefaultLanguage.Equals(language))
                return LoadCommandResult(DefaultLanguage);
            if (section == null)
                throw new Exception("Missing language file");
            loggerService.LogTrace("Loaded language file {language}", language);
            return section;
        }
        catch (Exception ex)
        {
            this.loggerService.LogError(ex, "Error loading language file {language}", language);
            throw;
        }
    }

    public string Text(string language, CommandResultEnum commandResultEnum, params object?[] values)
    {
        var section = LoadCommandResult(language);
        var text = section.GetValue<string>(commandResultEnum.ToString());
        if(text == null)
            throw new Exception($"Missing CommandResultEnum {commandResultEnum}");
        if(values.Length > 0)
            text = string.Format(text, values);
        return text;
    }

    public IConfiguration LoadCommandResult()
    {
        return LoadCommandResult(playerService.Language);
    }

    public string Text(CommandResultEnum commandResultEnum, params object?[] values)
    {
        return Text(playerService.Language, commandResultEnum, values);
    }
    public void SetResult(ICommand command, CommandResultEnum commandResultEnum, params object?[] values)
    {
        command.CommandResult = commandResultEnum;
        command.ExecuteResultText = this.Text(commandResultEnum, values);
    }

    public void SetResult(ICommand command, CommandResultEnum commandResultEnum, CommandResultEnum showText, params object?[] values)
    {
        command.CommandResult = commandResultEnum;
        command.ExecuteResultText = this.Text(showText, values);
    }
}
