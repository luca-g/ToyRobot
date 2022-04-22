using System.Text.Json.Serialization;

namespace ToyRobot.Common.Model;

public interface ICommandParameter
{
    string Name { get; }
    [JsonIgnore]
    Type ParameterType { get; }
    string ParameterTypeName { get => ParameterType.Name; }
    string Description();
    IList<string>? AcceptedValues { get; }
}
