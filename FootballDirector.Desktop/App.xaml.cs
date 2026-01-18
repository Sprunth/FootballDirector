using System.Windows;
using FootballDirector.Server;

namespace FootballDirector.Desktop;

public partial class App : Application
{
    private LocalGameHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        string contentRoot = AppContext.BaseDirectory;

#if DEBUG
        // In DEBUG, you can optionally point WebView2 to Vite dev server instead
        // by passing a different URI to MainWindow. For now, we start the local server.
        string env = "Development";
#else
        string env = "Production";
#endif

        try
        {
            _host = await LocalGameHost.StartAsync(contentRoot, env);

            var mainWindow = new MainWindow(_host.BaseUri);
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to start server: {ex.Message}", "Startup Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown(1);
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host != null)
        {
            await _host.DisposeAsync();
        }

        base.OnExit(e);
    }
}
