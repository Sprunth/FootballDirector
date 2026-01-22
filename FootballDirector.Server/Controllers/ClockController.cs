using FootballDirector.Contracts;
using FootballDirector.Core.Clock;
using Microsoft.AspNetCore.Mvc;

namespace FootballDirector.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClockController(GameClockService clockService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<GameClock>(StatusCodes.Status200OK)]
    public async Task<ActionResult<GameClock>> Get()
    {
        var clock = await clockService.GetCurrentClockAsync();
        return Ok(clock);
    }

    [HttpPost("advance")]
    [ProducesResponseType<GameClock>(StatusCodes.Status200OK)]
    public async Task<ActionResult<GameClock>> Advance([FromQuery] int days = 1)
    {
        var clock = await clockService.AdvanceDaysAsync(days);
        return Ok(clock);
    }
}
