using FootballDirector.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FootballDirector.Server.Controllers;

[ApiController]
[Route("api/footballers")]
public class FootballerController : ControllerBase
{
    private static readonly List<Footballer> Footballers =
    [
        new(1, "Danny", "Fletcher", 27, "England",
            new Personality(PersonalityType.Maverick, new Backstory(
                "Grew up on a council estate in Sheffield with a hardworking single mother.",
                "Scoring a hat-trick on his Premier League debut as an 18-year-old substitute.",
                "Runs a charity providing sports equipment to underprivileged schools.")),
            "LW", 81, 92, 78, 74, 85, 45, 72),
        new(2, "Pablo", "Moreno", 17, "Spain",
            new Personality(PersonalityType.Virtuoso, new Backstory(
                "Raised in a working-class neighborhood near Valencia by immigrant parents.",
                "Being scouted by a top academy at just 8 years old after a street football video went viral.",
                "Still lives with his grandmother who taught him his first skills with a tennis ball.")),
            "RW", 83, 88, 76, 79, 90, 32, 56),
        new(3, "Magnus", "Lindqvist", 24, "Norway",
            new Personality(PersonalityType.Warrior, new Backstory(
                "Born into a family of cross-country skiers in Trondheim who wanted him to follow tradition.",
                "Scoring 5 goals in a cup final after his team was down 2-0 at halftime.",
                "Practices meditation daily and is obsessed with sleep optimization and recovery routines.")),
            "ST", 91, 89, 93, 65, 80, 45, 88),
        new(4, "Tyler", "Chambers", 21, "England",
            new Personality(PersonalityType.Heartbeat, new Backstory(
                "Grew up in Birmingham with parents who ran a local youth football club.",
                "Captained his country's U-21 team to a European championship.",
                "Speaks fluent German after a teenage exchange program sparked his love of languages.")),
            "CAM", 88, 78, 82, 83, 86, 68, 78),
        new(5, "Luuk", "de Groot", 33, "Netherlands",
            new Personality(PersonalityType.Mentor, new Backstory(
                "Lost his father to illness when he was a teenager in Rotterdam.",
                "Being released by his boyhood club as a youngster and almost giving up on football.",
                "Plays chess competitively and uses it to sharpen his tactical reading of the game.")),
            "CB", 89, 62, 60, 72, 55, 92, 86),
        new(6, "Sergio", "Vidal", 28, "Spain",
            new Personality(PersonalityType.Strategist, new Backstory(
                "Grew up in a middle-class Seville family with parents who valued education.",
                "Winning Player of the Tournament after a dominant European Championship.",
                "Has a degree in economics and considered becoming a financial analyst.")),
            "CDM", 90, 58, 72, 88, 79, 88, 82),
        new(7, "Thierry", "Dubois", 26, "France",
            new Personality(PersonalityType.Showman, new Backstory(
                "Raised in the Paris suburbs by a father who coached amateur football and a mother who was a track athlete.",
                "Becoming a World Cup winner at 20 and gracing magazine covers worldwide.",
                "Donates his entire national team salary to charity and runs a foundation for youth athletics.")),
            "ST", 91, 97, 89, 80, 92, 36, 78),
        new(8, "Iker", "Ruiz", 22, "Spain",
            new Personality(PersonalityType.Introvert, new Backstory(
                "Grew up in a small coastal town in Galicia, far from mainland football academies.",
                "Playing over 60 matches in a single season at age 19 for club and country.",
                "Prefers staying home playing video games to going out, and is known for his quiet demeanor.")),
            "CM", 87, 72, 75, 88, 88, 72, 65),
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
