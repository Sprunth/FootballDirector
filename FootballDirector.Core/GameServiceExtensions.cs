using FootballDirector.Core.Data;
using FootballDirector.Core.LLM;
using Microsoft.Extensions.DependencyInjection;

namespace FootballDirector.Core;

/// <summary>
/// Consolidated game service registration. All clients (Server, Desktop, future) should use these methods.
/// </summary>
public static class GameServiceExtensions
{
    /// <summary>
    /// Registers all game services (database, LLM, etc.) with the DI container.
    /// Call this once during startup.
    /// </summary>
    public static IServiceCollection AddGameServices(this IServiceCollection services, string baseDirectory)
    {
        // Database
        var dbPath = Path.Combine(baseDirectory, "Data", "footballdirector.db");
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
        services.AddGameDatabase($"Data Source={dbPath}");

        // LLM
        var modelPath = Path.Combine(baseDirectory, "LLM", "LFM2.5-1.2B-Instruct-Q4_K_M.gguf");
        services.AddSingleton(new LlmTestService(modelPath));

        return services;
    }

    /// <summary>
    /// Initializes game state (creates database, seeds data, etc.).
    /// Call this after building the app, before running.
    /// </summary>
    public static void InitializeGame(this IServiceProvider services)
    {
        services.EnsureGameDatabaseCreated();
    }
}
