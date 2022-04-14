using Microsoft.Extensions.Configuration;
using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;

public interface IApplicationMessagesService
{
    IConfiguration LoadCommandResult(string language);
    string Text(string language, CommandResultEnum commandResultEnum, params object?[] values);
    void SetResult(string language, ICommand command, CommandResultEnum commandResultEnum, params object?[] values);
    void SetResult(string language, ICommand command, CommandResultEnum commandResultEnum, CommandResultEnum showText, params object?[] values);

}
