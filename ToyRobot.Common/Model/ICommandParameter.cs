namespace ToyRobot.Common.Model;

public interface ICommandParameter
{
    string Name { get; }
    Type ParameterType { get; }
    string Description();
}
