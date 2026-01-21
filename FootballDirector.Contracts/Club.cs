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
    ClubCounts Counts)
{
    // Required for EF Core to instantiate with owned types
    private Club() : this(0, "", "", "", 0, new ClubFinances(), new ClubCounts()) { }
}

/// <summary>
/// Financial snapshot of the club.
/// </summary>
public record ClubFinances(
    long Balance,               // Current bank balance
    long TransferBudget,        // Available for transfers
    long WageBudget,            // Weekly wage budget
    long CurrentWages)          // Current weekly wage spend
{
    // Required for EF Core
    public ClubFinances() : this(0, 0, 0, 0) { }
}

/// <summary>
/// Quick counts for dashboard display.
/// </summary>
public record ClubCounts(
    int Footballers,
    int Staff,
    int UnreadMessages)
{
    // Required for EF Core
    public ClubCounts() : this(0, 0, 0) { }
}
