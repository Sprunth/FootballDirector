using FootballDirector.Contracts;
using FootballDirector.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballDirector.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController(GameDbContext db) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<StaffMember>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StaffMember>>> GetAll([FromQuery] StaffRole? role = null)
    {
        var query = db.Staff.AsQueryable();
        if (role.HasValue)
            query = query.Where(s => s.Role == role.Value);

        var result = await query.ToListAsync();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<StaffMember>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StaffMember>> GetById(int id)
    {
        var staff = await db.Staff.FindAsync(id);
        if (staff is null)
            return NotFound();
        return Ok(staff);
    }
}
