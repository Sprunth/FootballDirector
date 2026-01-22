using FootballDirector.Core.Data;
using FootballDirector.Core.LLM;

namespace FootballDirector.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            // Register game database
            var dbPath = Path.Combine(AppContext.BaseDirectory, "Data", "footballdirector.db");
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
            builder.Services.AddGameDatabase($"Data Source={dbPath}");

            // Register LLM test service
            var modelPath = Path.Combine(AppContext.BaseDirectory, "LLM", "LFM2.5-1.2B-Instruct-Q4_K_M.gguf");
            modelPath = Path.GetFullPath(modelPath);
            builder.Services.AddSingleton(new LlmTestService(modelPath));

            var app = builder.Build();

            // Ensure database is created and seeded
            app.Services.EnsureGameDatabaseCreated();

            app.UseDefaultFiles();
            app.MapStaticAssets();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
