using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToyRobot.API.Model;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;


namespace ToyRobot.API.Controllers;

[Authorize]
[Route("api/v1/command")]
[ApiController]
public class CommandController : ControllerBase
{
    private readonly ICommandCenterService commandCenterService;
    private readonly ILogger<CommandController> logger;
    private readonly IFactoryService factoryService;
    public CommandController(
        ILogger<CommandController> logger,
        ICommandCenterService commandCenterService,
        IFactoryService factoryService)
    {
        this.logger = logger;
        this.commandCenterService = commandCenterService;
        this.factoryService = factoryService;
    }
    private Guid UserGuid { get { return Guid.Parse(HttpContext.User.Claims.First(t => t.Type == "userGuid").Value); } }

    [HttpGet]
    public ActionResult<IEnumerable<ICommandText>> Get()
    {
        try
        {
            logger.LogTrace("User {UserGuid} get command list", UserGuid);
            var commands = this.commandCenterService.Commands.Select(t => t.CommandInstructions);
            return Ok(commands);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "error getting command list");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
    [HttpPost]
    public async Task<IActionResult> Command(ExecuteCommandModel commandModel)
    {
        try
        {
            var userGuid = this.UserGuid;
            logger.LogTrace("Executing Command {userGuid},{MapId},{RobotId},{Text}", userGuid, commandModel.MapId, commandModel.RobotId, commandModel.Text);
            ExecuteCommandModel result = new();

            var scenario = await factoryService.CreateScenario(userGuid, commandModel.MapId, commandModel.RobotId);
            if(!string.IsNullOrWhiteSpace(commandModel.Text) && await this.commandCenterService.Execute(scenario, commandModel.Text))
            {
                result.Text = commandCenterService.ExecuteResult ?? "Invalid command";
            }
            else
            {
                result.Text = "Not executed";
            }
            result.MapId = scenario.MapId;
            result.RobotId = scenario.RobotId;

            logger.LogTrace("Executed Command {userGuid},{MapId},{RobotId},{Text}", userGuid, result.MapId, result.RobotId, result.Text);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "error executing command");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
    /*
    // GET api/<CommandController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<CommandController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<CommandController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<CommandController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
*/
}
