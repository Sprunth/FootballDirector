namespace FootballDirector.Contracts;

/// <summary>
/// Core club information and current state.
/// </summary>
public record Club(
    int Id,
    string Name,
    string Stadium,
    string League,
    int LeaguePosition,
    ClubFinances Finances,
    ClubCounts Counts);

/// <summary>
/// Financial snapshot of the club.
/// </summary>
public record ClubFinances(
    long Balance,               // Current bank balance
    long TransferBudget,        // Available for transfers
    long WageBudget,            // Weekly wage budget
    long CurrentWages);         // Current weekly wage spend

/// <summary>
/// Quick counts for dashboard display.
/// </summary>
public record ClubCounts(
    int Footballers,
    int Staff,
    int UnreadMessages);
