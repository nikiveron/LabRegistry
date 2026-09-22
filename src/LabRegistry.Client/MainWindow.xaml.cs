using LabRegistry.Client.Services;
using LabRegistry.Client.ViewModels;
using System.Net.Http;
using System.Windows;

namespace LabRegistry.Client;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(AppConfiguration.ApiBaseUrl)
        };

        var apiClient = new InspectionObjectsApiClient(httpClient);

        _viewModel = new MainViewModel(apiClient);

        DataContext = _viewModel;

        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        Loaded -= MainWindow_Loaded;

        await _viewModel.InitializeAsync();
    }
}