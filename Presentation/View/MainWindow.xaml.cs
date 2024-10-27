using System.Windows;
using NNTP_NEWS_CLIENT.Presentation.ViewModel;
using NNTP_NEWS_CLIENT.Application;
using NNTP_NEWS_CLIENT.Infrastructure;

namespace NNTP_NEWS_CLIENT.Presentation.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = ((App)App.Current);
        new SettingsViewModel();
        ViewModelController.Instance.SetCurrentViewModel(typeof(SettingsViewModel));
        
    }


    private void MenuItem_Settings_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel(typeof(SettingsViewModel));
    }


    private void MenuItem_Home_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModelController.Instance.SetCurrentViewModel(typeof(BrowserViewModel));
    }
}