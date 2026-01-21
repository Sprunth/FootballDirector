using FootballDirector.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FootballDirector.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClubController : ControllerBase
{
    // Hardcoded club data for now - will come from game state service later
    private static readonly Club CurrentClub = new(
        Id: 1,
        Name: "Ashworth United",
        Stadium: "Greenfield Park",
        League: "Premier Division",
        LeaguePosition: 7,
        Finances: new ClubFinances(
            Balance: 12_500_000,
            TransferBudget: 8_000_000,
            WageBudget: 450_000,
            CurrentWages: 385_000),
        Counts: new ClubCounts(
            Footballers: 8,     // Matches our hardcoded footballers
            Staff: 6,           // Matches our hardcoded staff
            UnreadMessages: 2));

    [HttpGet]
    [ProducesResponseType<Club>(StatusCodes.Status200OK)]
    public ActionResult<Club> Get()
    {
        return Ok(CurrentClub);
    }
}
