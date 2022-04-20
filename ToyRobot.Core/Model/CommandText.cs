using ToyRobot.Common.Model;

namespace ToyRobot.Core.Model;

public class CommandText : ICommandText
{
    public string CommandName { get; private set; }
    public string CommandHelp { get; private set; }
    public IList<ICommandParameter>? CommandParameters { get; private set; }
    public CommandText(string firstInstruction, IList<ICommandParameter>? commandParameters = null)
    {
        this.CommandName = firstInstruction;
        this.CommandParameters = commandParameters;
        this.CommandHelp = GetConsoleInstruction();
    }
    string GetConsoleInstruction()
    {
        List<string> parameters = new();
        if (CommandParameters != null && CommandParameters.Count > 0)
        {
            foreach (CommandParameter parameter in CommandParameters)
            {
                parameters.Add(parameter.Description());
            }
            var paramsList = String.Join(",", parameters);
            return $"{CommandName} {paramsList}";
        }
        else
        {
            return $"{CommandName}";
        }
    }
}
