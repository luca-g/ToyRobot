using Microsoft.Extensions.Configuration;
using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;

public interface IApplicationMessagesService
{
    IConfiguration LoadCommandResult();
    //string Text(CommandResultEnum commandResultEnum);
    string Text(CommandResultEnum commandResultEnum, params object?[] values);

    IConfiguration LoadCommandResult(string language);
    //string Text(string language, CommandResultEnum commandResultEnum);
    string Text(string language, CommandResultEnum commandResultEnum, params object?[] values);
    void SetResult(ICommand command, CommandResultEnum commandResultEnum, params object?[] values);
    void SetResult(ICommand command, CommandResultEnum commandResultEnum, CommandResultEnum showText, params object?[] values);

}
