using FootballDirector.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FootballDirector.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private static readonly List<StaffMember> StaffMembers =
    [
        // Manager
        new(100, "Roberto", "Santini", 52, "Italy",
            new Personality(PersonalityType.Strategist, new Backstory(
                "Grew up in a small town near Milan, son of a factory worker who never missed a Sunday match.",
                "Leading a struggling Serie B team to promotion and a domestic cup final in the same season.",
                "Collects vintage tactical boards and has an impressive library of football philosophy books.")),
            StaffRole.Manager,
            Specialization: null,
            Attacking: null, Defending: null, Goalkeeping: null, Tactics: 17, Communication: null,
            ManManagement: 15, Motivation: 16, MediaHandling: 12,
            InjuryPrevention: null, Recovery: null,
            JudgingAbility: null, JudgingPotential: null,
            BusinessAcumen: null, Negotiation: null,
            Wealth: null, Ambition: null),

        // Attacking Coach
        new(101, "Yuki", "Nakamura", 38, "Japan",
            new Personality(PersonalityType.Virtuoso, new Backstory(
                "Raised in Osaka by parents who ran a youth football academy.",
                "Developing three players who went on to play for the national team.",
                "Creates detailed video analysis packages set to classical music for player motivation.")),
            StaffRole.Coach,
            Specialization: CoachSpecialization.Attacking,
            Attacking: 16, Defending: 10, Goalkeeping: 5, Tactics: 14, Communication: 15,
            ManManagement: null, Motivation: null, MediaHandling: null,
            InjuryPrevention: null, Recovery: null,
            JudgingAbility: null, JudgingPotential: null,
            BusinessAcumen: null, Negotiation: null,
            Wealth: null, Ambition: null),

        // Defensive Coach
        new(102, "Graham", "Whitmore", 45, "England",
            new Personality(PersonalityType.Mentor, new Backstory(
                "Former professional defender who played over 400 matches in the lower leagues.",
                "Keeping a record 12 clean sheets during his playing days in a single season.",
                "Runs a popular podcast interviewing legendary defenders about the art of defending.")),
            StaffRole.Coach,
            Specialization: CoachSpecialization.Defending,
            Attacking: 8, Defending: 18, Goalkeeping: 7, Tactics: 15, Communication: 14,
            ManManagement: null, Motivation: null, MediaHandling: null,
            InjuryPrevention: null, Recovery: null,
            JudgingAbility: null, JudgingPotential: null,
            BusinessAcumen: null, Negotiation: null,
            Wealth: null, Ambition: null),

        // Head Scout
        new(103, "Maria", "Ferreira", 41, "Portugal",
            new Personality(PersonalityType.Strategist, new Backstory(
                "Daughter of a legendary Portuguese scout who discovered several world-class talents.",
                "Recommending a player for £500k who was later sold for £40 million.",
                "Speaks six languages fluently and travels over 200 days per year watching football.")),
            StaffRole.Scout,
            Specialization: null,
            Attacking: null, Defending: null, Goalkeeping: null, Tactics: null, Communication: null,
            ManManagement: null, Motivation: null, MediaHandling: null,
            InjuryPrevention: null, Recovery: null,
            JudgingAbility: 17, JudgingPotential: 19,
            BusinessAcumen: null, Negotiation: null,
            Wealth: null, Ambition: null),

        // Head Physio
        new(104, "Anders", "Bergström", 36, "Sweden",
            new Personality(PersonalityType.Heartbeat, new Backstory(
                "Former sports science student who specialized in elite athlete rehabilitation.",
                "Helping a star player return from a career-threatening injury ahead of schedule.",
                "Practices yoga daily and has completed multiple ultramarathons.")),
            StaffRole.Physio,
            Specialization: null,
            Attacking: null, Defending: null, Goalkeeping: null, Tactics: null, Communication: null,
            ManManagement: null, Motivation: null, MediaHandling: null,
            InjuryPrevention: 16, Recovery: 18,
            JudgingAbility: null, JudgingPotential: null,
            BusinessAcumen: null, Negotiation: null,
            Wealth: null, Ambition: null),

        // Chief Executive
        new(105, "Victoria", "Ashworth", 48, "England",
            new Personality(PersonalityType.Strategist, new Backstory(
                "Former investment banker who fell in love with football through her children's youth teams.",
                "Negotiating a stadium naming rights deal worth three times the previous valuation.",
                "The club was founded by her great-grandfather, making her the fourth generation to lead it.")),
            StaffRole.ChiefExecutive,
            Specialization: null,
            Attacking: null, Defending: null, Goalkeeping: null, Tactics: null, Communication: null,
            ManManagement: null, Motivation: null, MediaHandling: null,
            InjuryPrevention: null, Recovery: null,
            JudgingAbility: null, JudgingPotential: null,
            BusinessAcumen: 17, Negotiation: 15,
            Wealth: null, Ambition: null),
    ];

    [HttpGet]
    [ProducesResponseType<IEnumerable<StaffMember>>(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<StaffMember>> GetAll([FromQuery] StaffRole? role = null)
    {
        var result = role.HasValue
            ? StaffMembers.Where(s => s.Role == role.Value)
            : StaffMembers;
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<StaffMember>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<StaffMember> GetById(int id)
    {
        var staff = StaffMembers.FirstOrDefault(s => s.Id == id);
        if (staff is null)
            return NotFound();
        return Ok(staff);
    }
}
