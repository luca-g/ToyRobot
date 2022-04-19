namespace ToyRobot.Common.Services;

public interface IAuthenticationService
{
    string GenerateSecurityToken(Guid playerGuid);
}
