using ToyRobot.Common.Model;
using ToyRobot.Core.Model;

namespace ToyRobot.SqlServerModel.DB;

public partial class Robot : IRobot
{
    IPlayer IRobot.Player => (IPlayer) this.Player;

    IMap IRobot.Map => (IMap) this.Map;

    IMapPosition? IRobot.Position { 
        get {
            if (this.X != null && this.Y != null && this.OrientationId != null)
            {
                return new MapPosition(this.X.Value, this.Y.Value, (MapOrientationEnum) this.OrientationId);
            }
            return null;
        } 
    } 

    Task IRobot.SetMapPosition(IMapPosition? mapPosition)
    {
        if(mapPosition == null)
        {
            this.OrientationId = null;
            this.X = null;
            this.Y = null;
        }
        else
        {
            this.OrientationId = (int)mapPosition.Orientation;
            this.X = mapPosition.X;
            this.Y = mapPosition.Y;
        }
        return Task.CompletedTask;
    }
}