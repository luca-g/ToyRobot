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
        public UserController(IPlayerService playerService)
        {
            this.playerService = playerService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            try
            {
                var player = await this.playerService.LoadPlayer(loginModel.UserGuid);
                return Ok(player);
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
        public async Task<IActionResult> Create()
        {
            try
            {
                var player = await this.playerService.CreatePlayer();
                return Ok(player);
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
