namespace ToyRobot.API.Model;

public class CreateUserModel
{
    public Guid UserGuid { get; set; }
    public string Token { get; set; } = string.Empty;
}
