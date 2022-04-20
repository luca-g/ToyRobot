using ToyRobot.Common.Model;

namespace ToyRobot.Core.Model;

internal class CommandParameter : ICommandParameter
{
    public string Name {get; set;}

    public Type ParameterType { get; set;}
    public string Description()
    {        
        if(ParameterType.IsEnum)
        {
            var values = new List<string>();
            foreach(var value in Enum.GetValues(ParameterType))
            {
                values.Add(value.ToString() ?? String.Empty);
            }
            var valuesDescription = String.Join(",", values);
            return $"{Name} ({valuesDescription})";
        }
        return Name;
    }
    public CommandParameter(string name, Type type)
    {
        this.Name = name;
        this.ParameterType = type;
    }
}
