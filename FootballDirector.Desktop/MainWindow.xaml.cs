using System.Windows;

namespace FootballDirector.Desktop;

public partial class MainWindow : Window
{
    private readonly LocalGameHost _gameHost = new();

    public MainWindow()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Closed += OnClosed;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        await _gameHost.StartAsync();
        await webView.EnsureCoreWebView2Async();
        webView.CoreWebView2.Navigate(_gameHost.BaseUrl);
    }

    private async void OnClosed(object? sender, EventArgs e)
    {
        await _gameHost.StopAsync();
        _gameHost.Dispose();
    }
}
