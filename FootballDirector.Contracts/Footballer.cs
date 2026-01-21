namespace FootballDirector.Contracts;

/// <summary>
/// A football player with attributes and personality.
/// </summary>
public record Footballer(
    int Id,
    string FirstName,
    string LastName,
    int Age,
    string Nationality,
    Personality Personality,
    string Position,
    int OverallRating,      // 1-99: overall skill level
    int Pace,               // 1-99: speed and acceleration
    int Shooting,           // 1-99: finishing and shot power
    int Passing,            // 1-99: short and long passing
    int Dribbling,          // 1-99: ball control and skill moves
    int Defending,          // 1-99: tackling and positioning
    int Physical            // 1-99: strength and stamina
) : Person(Id, FirstName, LastName, Age, Nationality, Personality)
{
    // Required for EF Core to instantiate with owned types
    private Footballer() : this(0, "", "", 0, "", new Personality(), "", 0, 0, 0, 0, 0, 0, 0) { }
}
