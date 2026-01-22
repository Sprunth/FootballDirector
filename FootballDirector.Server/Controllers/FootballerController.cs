using FootballDirector.Contracts;
using FootballDirector.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballDirector.Server.Controllers;

[ApiController]
[Route("api/footballers")]
public class FootballerController(GameDbContext db) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<Footballer>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Footballer>>> GetAll()
    {
        var footballers = await db.Footballers.ToListAsync();
        return Ok(footballers);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<Footballer>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Footballer>> GetById(int id)
    {
        var footballer = await db.Footballers.FindAsync(id);
        if (footballer is null)
            return NotFound();
        return Ok(footballer);
    }
}
