using ToyRobot.Common.Model;

namespace ToyRobot.Core.Model;

public class CommandTextBuilder : ICommandTextBuilder
{
    string firstInstruction = string.Empty;
    readonly List<ICommandParameter> parameters = new();
    public ICommandTextBuilder AddCommandParameter(string parameterName, Type parameterType)
    {
        parameters.Add(new CommandParameter(parameterName, parameterType));
        return this;
    }

    public ICommandText Build()
    {
        if(this.firstInstruction.Equals(string.Empty))
        {
            throw new Exception("firstInstruction not set");
        }
        var commandInstructions = new CommandText(firstInstruction, parameters);
        return commandInstructions;
    }

    public ICommandTextBuilder SetFirstInstruction(string firstInstruction)
    {
        this.firstInstruction = firstInstruction;
        return this;
    }
}
