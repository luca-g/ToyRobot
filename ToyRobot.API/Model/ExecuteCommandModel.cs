namespace ToyRobot.API.Model
{
    public class ExecuteCommandModel
    {
        public int? MapId { get; set; }
        public int? RobotId { get; set; }
        public string? Text { get; set; }
    }
}
