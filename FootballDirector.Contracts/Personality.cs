namespace FootballDirector.Contracts;

/// <summary>
/// Preset personality archetypes that influence NPC behavior and dialogue.
/// </summary>
public enum PersonalityType
{
    Maverick,       // Unpredictable, creative, takes risks
    Virtuoso,       // Perfectionist, technically obsessed, detail-oriented
    Heartbeat,      // Team player, emotional leader, glue guy
    Mentor,         // Nurturing, patient, focused on development
    Warrior,        // Combative, never gives up, leads by example
    Strategist,     // Analytical, calm under pressure, chess-player mentality
    Showman,        // Loves the spotlight, media-friendly, entertainer
    Introvert       // Quiet, lets actions speak, dislikes media
}

/// <summary>
/// A three-sentence backstory generated for each character.
/// Used by LLM to inform NPC personality and dialogue.
/// </summary>
/// <param name="Upbringing">The household/environment they grew up in.</param>
/// <param name="CoreMemory">A standout life event that shaped them.</param>
/// <param name="FunFact">An interesting personal detail or quirk.</param>
public record Backstory(
    string Upbringing,
    string CoreMemory,
    string FunFact)
{
    // Required for EF Core
    public Backstory() : this("", "", "") { }
}

/// <summary>
/// Complete personality profile for any person in the game.
/// </summary>
public record Personality(
    PersonalityType Type,
    Backstory Backstory)
{
    // Required for EF Core
    public Personality() : this(PersonalityType.Maverick, new Backstory()) { }
}
