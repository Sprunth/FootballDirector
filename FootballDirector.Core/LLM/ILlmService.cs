namespace FootballDirector.Core.LLM;

/// <summary>
/// Abstraction for LLM services allowing swapping between local and cloud providers.
/// </summary>
public interface ILlmService : IAsyncDisposable
{
    /// <summary>
    /// Generates a response from the LLM.
    /// </summary>
    Task<LlmResponse> GenerateAsync(LlmRequest request, CancellationToken ct = default);

    /// <summary>
    /// Indicates whether the LLM service is available and ready to use.
    /// </summary>
    bool IsAvailable { get; }
}
