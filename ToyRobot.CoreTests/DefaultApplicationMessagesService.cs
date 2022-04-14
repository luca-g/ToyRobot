using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Tests;

internal class DefaultApplicationMessagesService : IApplicationMessagesService
{
    class DefaultConfiguration : IConfiguration
    {
        public string this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }
    }
#pragma warning disable CA1822 // Mark members as static
    public IConfiguration LoadCommandResult()
#pragma warning restore CA1822 // Mark members as static
    {
        return new DefaultConfiguration();
    }

    public IConfiguration LoadCommandResult(string language)
    {
        return new DefaultConfiguration();
    }

    public void SetResult(string language, ICommand command, CommandResultEnum commandResultEnum, params object?[] values)
    {
        command.CommandResult = commandResultEnum;
        command.ExecuteResultText = commandResultEnum.ToString();
    }

    public void SetResult(string language, ICommand command, CommandResultEnum commandResultEnum, CommandResultEnum showText, params object?[] values)
    {
        command.CommandResult = commandResultEnum;
        command.ExecuteResultText = showText.ToString();
    }

    public string Text(string language, CommandResultEnum commandResultEnum, params object?[] values)
    {
        return commandResultEnum.ToString();
    }
}
