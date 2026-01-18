using FootballDirector.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FootballDirector.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FootballerController : ControllerBase
{
    private static readonly List<Footballer> Footballers =
    [
        new(1, "Marcus", "Rashford", 27, "LW", "England", 81, 92, 78, 74, 85, 45, 72),
        new(2, "Lamine", "Yamal", 17, "RW", "Spain", 83, 88, 76, 79, 90, 32, 56),
        new(3, "Erling", "Haaland", 24, "ST", "Norway", 91, 89, 93, 65, 80, 45, 88),
        new(4, "Jude", "Bellingham", 21, "CAM", "England", 88, 78, 82, 83, 86, 68, 78),
        new(5, "Virgil", "van Dijk", 33, "CB", "Netherlands", 89, 62, 60, 72, 55, 92, 86),
        new(6, "Rodri", "Hernandez", 28, "CDM", "Spain", 90, 58, 72, 88, 79, 88, 82),
        new(7, "Kylian", "Mbappe", 26, "ST", "France", 91, 97, 89, 80, 92, 36, 78),
        new(8, "Pedri", "Gonzalez", 22, "CM", "Spain", 87, 72, 75, 88, 88, 72, 65),
    ];

    [HttpGet]
    [ProducesResponseType<IEnumerable<Footballer>>(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Footballer>> GetAll()
    {
        return Ok(Footballers);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<Footballer>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Footballer> GetById(int id)
    {
        var footballer = Footballers.FirstOrDefault(f => f.Id == id);
        if (footballer is null)
            return NotFound();
        return Ok(footballer);
    }
}
