namespace FootballDirector.Core.LLM;

public class LlmTestService
{
    private readonly string _modelPath;

    public LlmTestService(string modelPath)
    {
        _modelPath = modelPath;
    }

    public async Task<LlmResponse> GenerateFootballCharacterAsync(CancellationToken ct = default)
    {
        await using var llm = new LlamaSharpService(_modelPath);

        if (!llm.IsAvailable)
        {
            return new LlmResponse(string.Empty, false, $"Model file not found: {_modelPath}");
        }

        var request = new LlmRequest(
            Prompt: "Generate a fictional football player. Include their name, age, nationality, position, and a brief personality description. Keep it to 2-3 sentences.",
            SystemPrompt: "You are a creative writer for a football management game. Be concise.",
            Temperature: 0.8f,
            MaxTokens: 150
        );

        return await llm.GenerateAsync(request, ct);
    }
}
