using FootballDirector.Contracts;

namespace FootballDirector.Core.Clock;

/// <summary>
/// Extension methods for calculating age and birthday-related logic.
/// </summary>
public static class PersonExtensions
{
    /// <summary>
    /// Calculates the person's age as of a specific date.
    /// </summary>
    public static int GetAge(this Person person, DateTime asOfDate)
    {
        var age = asOfDate.Year - person.DateOfBirth.Year;
        if (asOfDate < person.DateOfBirth.AddYears(age))
            age--;
        return age;
    }

    /// <summary>
    /// Checks if the person has a birthday on the specified date.
    /// </summary>
    public static bool HasBirthdayOn(this Person person, DateTime date)
        => person.DateOfBirth.Month == date.Month
        && person.DateOfBirth.Day == date.Day;
}
