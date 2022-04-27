using Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToyRobot.API.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.API.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IPlayerService playerService;
        private readonly IJwtService jwtService;
        public UserController(IPlayerService playerService, IJwtService jwtService)
        {
            this.playerService = playerService;
            this.jwtService = jwtService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginModel loginModel)
        {
            try
            {
                var player = await this.playerService.LoadPlayer(loginModel.UserGuid);
                var token = jwtService.CreateToken(new Dictionary<string, object>
                {
                    { "userGuid", player.PlayerGuid }
                });
                return Ok(token);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch(Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("create")]
        public async Task<ActionResult<CreateUserModel>> Create()
        {
            try
            {
                var player = await this.playerService.CreatePlayer();
                var token = jwtService.CreateToken(new Dictionary<string, object>
                {
                    { "userGuid", player.PlayerGuid }
                });
                var result = new CreateUserModel
                {
                    Token = token,
                    UserGuid = player.PlayerGuid
                };
                return Ok(result);
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
