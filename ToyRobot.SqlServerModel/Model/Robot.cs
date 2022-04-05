using ToyRobot.Common.Model;
using ToyRobot.Core.Model;

namespace ToyRobot.SqlServerModel.DB;

public partial class Robot : IRobot
{
    IMapPosition? IRobot.Position { 
        get {
            if (this.X != null && this.Y != null && this.OrientationId != null)
            {
                return new MapPosition(this.X.Value, this.Y.Value, (MapOrientationEnum) this.OrientationId);
            }
            return null;
        }
        set
        {
            this.SetMapPosition(value);
        }
    }

    private void SetMapPosition(IMapPosition? mapPosition)
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
    }
}