namespace FootballDirector.Contracts;

/// <summary>
/// Specialization area for coaches.
/// </summary>
public enum CoachSpecialization
{
    Attacking,
    Defending,
    Goalkeeping,
    Fitness,
    SetPiece
}

/// <summary>
/// Assistant/specialist coach with focused expertise.
/// </summary>
public record Coach(
    int Id,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Nationality,
    Personality Personality,
    CoachSpecialization Specialization,
    int Attacking,          // 1-20: ability to develop attacking play
    int Defending,          // 1-20: ability to develop defensive structure
    int Goalkeeping,        // 1-20: ability to train goalkeepers
    int Tactics,            // 1-20: tactical understanding
    int Communication       // 1-20: ability to convey ideas to players
) : Person(Id, FirstName, LastName, DateOfBirth, Nationality, Personality);

/// <summary>
/// Head manager/head coach - the main football decision maker.
/// </summary>
public record Manager(
    int Id,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Nationality,
    Personality Personality,
    int Tactics,            // 1-20: tactical acumen
    int ManManagement,      // 1-20: handling player egos and morale
    int Motivation,         // 1-20: inspiring performances
    int MediaHandling       // 1-20: press conferences, public image
) : Person(Id, FirstName, LastName, DateOfBirth, Nationality, Personality);

/// <summary>
/// Medical staff responsible for player fitness and recovery.
/// </summary>
public record Physio(
    int Id,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Nationality,
    Personality Personality,
    int InjuryPrevention,   // 1-20: reducing injury likelihood
    int Recovery            // 1-20: speeding up recovery times
) : Person(Id, FirstName, LastName, DateOfBirth, Nationality, Personality);

/// <summary>
/// Talent identification specialist.
/// </summary>
public record Scout(
    int Id,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Nationality,
    Personality Personality,
    int JudgingAbility,     // 1-20: assessing current skill level
    int JudgingPotential    // 1-20: predicting future development
) : Person(Id, FirstName, LastName, DateOfBirth, Nationality, Personality);

/// <summary>
/// Club CEO - handles business operations.
/// </summary>
public record ChiefExecutive(
    int Id,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Nationality,
    Personality Personality,
    int BusinessAcumen,     // 1-20: commercial and financial decisions
    int Negotiation         // 1-20: deal-making ability
) : Person(Id, FirstName, LastName, DateOfBirth, Nationality, Personality);

/// <summary>
/// Club owner - the money and ultimate authority.
/// </summary>
public record ClubOwner(
    int Id,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Nationality,
    Personality Personality,
    long Wealth,            // Net worth in currency units
    int Ambition            // 1-20: willingness to invest and push for success
) : Person(Id, FirstName, LastName, DateOfBirth, Nationality, Personality);
