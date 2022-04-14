using System.ComponentModel.DataAnnotations;

namespace ToyRobot.API.Model;

public class LoginModel
{
    [Required]
    public Guid UserGuid { get; set; }

}
