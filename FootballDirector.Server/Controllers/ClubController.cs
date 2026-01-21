using FootballDirector.Contracts;
using FootballDirector.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballDirector.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubController(GameDbContext db) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<Club>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Club>> Get()
    {
        var club = await db.Clubs.FirstOrDefaultAsync();
        if (club is null)
            return NotFound();
        return Ok(club);
    }
}
