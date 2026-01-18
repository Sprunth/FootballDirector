namespace FootballDirector.Contracts;

public record Footballer(
    int Id,
    string FirstName,
    string LastName,
    int Age,
    string Position,
    string Nationality,
    int OverallRating,
    int Pace,
    int Shooting,
    int Passing,
    int Dribbling,
    int Defending,
    int Physical);
