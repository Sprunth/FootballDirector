namespace FootballDirector.Contracts;

/// <summary>
/// Staff role discriminator for API responses.
/// </summary>
public enum StaffRole
{
    Coach,
    Manager,
    Physio,
    Scout,
    ChiefExecutive,
    ClubOwner
}

/// <summary>
/// Unified staff response for API - flattens all staff types into one shape.
/// Attributes are nullable since different roles have different stats.
/// </summary>
public record StaffMember(
    int Id,
    string FirstName,
    string LastName,
    int Age,
    string Nationality,
    Personality Personality,
    StaffRole Role,

    // Coach-specific
    CoachSpecialization? Specialization,

    // Shared coaching/tactical stats (Coach, Manager)
    int? Attacking,
    int? Defending,
    int? Goalkeeping,
    int? Tactics,
    int? Communication,

    // Manager-specific
    int? ManManagement,
    int? Motivation,
    int? MediaHandling,

    // Physio-specific
    int? InjuryPrevention,
    int? Recovery,

    // Scout-specific
    int? JudgingAbility,
    int? JudgingPotential,

    // ChiefExecutive-specific
    int? BusinessAcumen,
    int? Negotiation,

    // ClubOwner-specific
    long? Wealth,
    int? Ambition)
{
    // Required for EF Core to instantiate with owned types
    private StaffMember() : this(0, "", "", 0, "", new Personality(), StaffRole.Coach,
        null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null) { }
}
