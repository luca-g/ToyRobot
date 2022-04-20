using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRobot.Common.Model
{
    public interface ICommandText
    {
        string CommandName { get; }
        string CommandHelp { get; }
        IList<ICommandParameter>? CommandParameters { get; }
    }
}
