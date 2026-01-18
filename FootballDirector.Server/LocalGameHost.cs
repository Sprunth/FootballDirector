using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace FootballDirector.Server;

/// <summary>
/// Wrapper for hosting the ASP.NET Core server in-process.
/// </summary>
public sealed class LocalGameHost : IAsyncDisposable
{
    private readonly WebApplication _app;

    public Uri BaseUri { get; }

    private LocalGameHost(WebApplication app, Uri baseUri)
    {
        _app = app;
        BaseUri = baseUri;
    }

    /// <summary>
    /// Starts the local game server on localhost with a free port.
    /// </summary>
    /// <param name="contentRoot">The content root path (usually AppContext.BaseDirectory).</param>
    /// <param name="environmentName">The environment name (e.g., "Development", "Production").</param>
    /// <param name="port">Optional port. If 0 or not specified, a free port is selected.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A LocalGameHost instance with BaseUri and disposable handle.</returns>
    public static async Task<LocalGameHost> StartAsync(
        string contentRoot,
        string? environmentName = null,
        int port = 0,
        CancellationToken cancellationToken = default)
    {
        if (port == 0)
        {
            port = GetFreePort();
        }

        var webRoot = Path.Combine(contentRoot, "wwwroot");

        var options = new WebApplicationOptions
        {
            ContentRootPath = contentRoot,
            WebRootPath = webRoot,
            EnvironmentName = environmentName ?? "Production"
        };

        var builder = WebApplication.CreateBuilder(options);

        // Configure Kestrel to listen on localhost only
        builder.WebHost.ConfigureKestrel(kestrel =>
        {
            kestrel.ListenLocalhost(port);
        });

        // Add services
        builder.Services.AddControllers();

        // In development, add OpenAPI support
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddOpenApi();
        }

        var app = builder.Build();

        // Configure pipeline for SPA
        app.UseDefaultFiles();
        app.UseStaticFiles();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseAuthorization();

        // Map API endpoints
        app.MapControllers();

        // SPA fallback - serve index.html for client-side routes
        app.MapFallbackToFile("index.html");

        await app.StartAsync(cancellationToken);

        var baseUri = new Uri($"http://127.0.0.1:{port}/");
        return new LocalGameHost(app, baseUri);
    }

    /// <summary>
    /// Finds an available TCP port on localhost.
    /// </summary>
    private static int GetFreePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }

    public async ValueTask DisposeAsync()
    {
        await _app.StopAsync();
        await _app.DisposeAsync();
    }
}
