using FootballDirector.Contracts;
using FootballDirector.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballDirector.Core.Clock;

/// <summary>
/// Service for managing game time progression.
/// </summary>
public class GameClockService
{
    private readonly GameDbContext _db;

    public GameClockService(GameDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Gets the current game clock state.
    /// </summary>
    public async Task<GameClock> GetCurrentClockAsync()
    {
        return await _db.GameClock.SingleAsync();
    }

    /// <summary>
    /// Advances the game clock by the specified number of days.
    /// </summary>
    public async Task<GameClock> AdvanceDaysAsync(int days)
    {
        var clock = await _db.GameClock.SingleAsync();
        var newDate = clock.CurrentDate.AddDays(days);
        var newPhase = DeterminePhase(newDate);
        var newSeason = DetermineSeason(newDate);

        var updated = clock with
        {
            CurrentDate = newDate,
            Phase = newPhase,
            Season = newSeason
        };

        _db.Entry(clock).CurrentValues.SetValues(updated);
        await _db.SaveChangesAsync();

        return updated;
    }

    /// <summary>
    /// Advances to the next significant date (stub for now - just advances 1 day).
    /// </summary>
    public async Task<GameClock> AdvanceToNextEventAsync()
    {
        // TODO: When we have scheduled events, skip to the next one
        return await AdvanceDaysAsync(1);
    }

    private static SeasonPhase DeterminePhase(DateTime date)
    {
        var month = date.Month;
        return month switch
        {
            7 or 8 => SeasonPhase.PreSeason,
            >= 9 and <= 12 => SeasonPhase.EarlySeason,
            1 => SeasonPhase.TransferWindow,
            >= 2 and <= 4 => SeasonPhase.LateSeason,
            5 or 6 => SeasonPhase.EndOfSeason,
            _ => SeasonPhase.PreSeason
        };
    }

    private static int DetermineSeason(DateTime date)
    {
        // Football season spans two calendar years (e.g., 2024/25 season)
        // Season year is the year it starts in (July onwards = that year, Jan-June = previous year)
        return date.Month >= 7 ? date.Year : date.Year - 1;
    }
}
