namespace ToyRobot.Common.Model;

public interface ICommandTextBuilder
{
    ICommandTextBuilder SetFirstInstruction(string firstInstruction);
    ICommandTextBuilder AddCommandParameter(string parameterName, Type parameterType);
    ICommandText Build();
}
