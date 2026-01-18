using System.Windows;

namespace FootballDirector.Desktop;

public partial class MainWindow : Window
{
    private readonly Uri _baseUri;

    public MainWindow(Uri baseUri)
    {
        _baseUri = baseUri;
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        await webView.EnsureCoreWebView2Async();
        webView.Source = _baseUri;
    }
}
