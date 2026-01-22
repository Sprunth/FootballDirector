namespace FootballDirector.Contracts;

/// <summary>
/// The current game time state. Singleton entity (always Id=1).
/// </summary>
public record GameClock(
    int Id,                 // Always 1 (singleton)
    DateTime CurrentDate,   // Current in-game date
    int Season,             // Current season year (e.g., 2024)
    SeasonPhase Phase);

public enum SeasonPhase
{
    PreSeason,          // July-August
    EarlySeason,        // August-December
    TransferWindow,     // January
    LateSeason,         // February-May
    EndOfSeason         // May-June
}
