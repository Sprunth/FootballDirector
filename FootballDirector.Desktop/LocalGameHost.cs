using System.IO;
using System.Net;
using System.Net.Sockets;
using FootballDirector.Core.LLM;
using FootballDirector.Server.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FootballDirector.Desktop;

public class LocalGameHost : IDisposable
{
    private WebApplication? _app;

    public string BaseUrl { get; private set; } = string.Empty;

    public async Task StartAsync()
    {
        var port = FindFreePort();
        BaseUrl = $"http://localhost:{port}";

        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls(BaseUrl);
        builder.Environment.WebRootPath = Path.Combine(AppContext.BaseDirectory, "wwwroot");

        // Add services - include controllers from Server assembly
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(LlmController).Assembly);

        var modelPath = Path.Combine(AppContext.BaseDirectory, "LLM", "LFM2.5-1.2B-Instruct-Q4_K_M.gguf");
        modelPath = Path.GetFullPath(modelPath);
        builder.Services.AddSingleton(new LlmTestService(modelPath));

        _app = builder.Build();

        _app.UseDefaultFiles();
        _app.UseStaticFiles();
        _app.UseAuthorization();
        _app.MapControllers();
        _app.MapFallbackToFile("/index.html");

        await _app.StartAsync();
    }

    public async Task StopAsync()
    {
        if (_app != null)
        {
            await _app.StopAsync();
        }
    }

    public void Dispose()
    {
        _app?.DisposeAsync().AsTask().Wait();
    }

    private static int FindFreePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}
