namespace FootballDirector.Core.LLM;

/// <summary>
/// Response model from LLM generation.
/// </summary>
/// <param name="Text">The generated text response.</param>
/// <param name="Success">Whether the generation completed successfully.</param>
/// <param name="Error">Error message if generation failed.</param>
public record LlmResponse(
    string Text,
    bool Success,
    string? Error = null);
