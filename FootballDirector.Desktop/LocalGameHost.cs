using System.IO;
using System.Net;
using System.Net.Sockets;
using FootballDirector.Core;
using FootballDirector.Server.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

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

        // Add controllers from Server assembly + all game services
        builder.Services.AddControllers()
            .AddApplicationPart(typeof(LlmController).Assembly);
        builder.Services.AddGameServices(AppContext.BaseDirectory);

        _app = builder.Build();
        _app.Services.InitializeGame();

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
