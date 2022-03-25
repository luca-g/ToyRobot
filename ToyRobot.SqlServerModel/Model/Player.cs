using ToyRobot.Common.Model;

namespace ToyRobot.SqlServerModel.DB
{
    public partial class Player : IPlayer
    {
        public Guid PlayerGuid => this.Identifier;
    }
}