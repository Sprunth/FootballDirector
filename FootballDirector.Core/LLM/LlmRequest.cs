namespace FootballDirector.Core.LLM;

/// <summary>
/// Request model for LLM generation.
/// </summary>
/// <param name="Prompt">The user prompt to send to the LLM.</param>
/// <param name="SystemPrompt">Optional system prompt for context/instructions.</param>
/// <param name="Temperature">Sampling temperature (0.0-1.0). Higher values produce more random output.</param>
/// <param name="MaxTokens">Maximum number of tokens to generate.</param>
public record LlmRequest(
    string Prompt,
    string? SystemPrompt = null,
    float Temperature = 0.7f,
    int MaxTokens = 256);
