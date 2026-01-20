namespace FootballDirector.Contracts;

/// <summary>
/// Base record for all people in the game (players, staff, executives).
/// </summary>
public abstract record Person(
    int Id,
    string FirstName,
    string LastName,
    int Age,
    string Nationality,
    Personality Personality);
