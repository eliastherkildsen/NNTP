using System.Windows;
using WPF_MVVM_TEMPLATE.Application;
using WPF_MVVM_TEMPLATE.Infrastructure;
using WPF_MVVM_TEMPLATE.Presentation.ViewModel;

namespace WPF_MVVM_TEMPLATE.Presentation.View;

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
        new BrowserViewModel();
        ViewModelController.Instance.SetCurrentViewModel(typeof(BrowserViewModel));
        
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