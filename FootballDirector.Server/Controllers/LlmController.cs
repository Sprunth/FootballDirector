using FootballDirector.Core.LLM;
using Microsoft.AspNetCore.Mvc;

namespace FootballDirector.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LlmController : ControllerBase
{
    private readonly LlmTestService _llmTestService;
    private readonly ILogger<LlmController> _logger;

    public LlmController(LlmTestService llmTestService, ILogger<LlmController> logger)
    {
        _llmTestService = llmTestService;
        _logger = logger;
    }

    [HttpPost("test")]
    public async Task<IActionResult> TestGeneration(CancellationToken ct)
    {
        _logger.LogInformation("LLM test request received");

        try
        {
            var response = await _llmTestService.GenerateFootballCharacterAsync(ct);

            _logger.LogInformation("LLM response - Success: {Success}, Text length: {Length}, Error: {Error}",
                response.Success, response.Text?.Length ?? 0, response.Error);

            if (response.Success)
            {
                _logger.LogInformation("Returning OK with text: {Text}", response.Text);
                return Ok(new { text = response.Text });
            }

            _logger.LogWarning("LLM generation failed: {Error}", response.Error);
            return StatusCode(500, new { error = response.Error });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in LLM test endpoint");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
