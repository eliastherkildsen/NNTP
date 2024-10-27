using System.Windows;
using System.Windows.Input;
using NNTP_NEWS_CLIENT.Application;
using NNTP_NEWS_CLIENT.InterfaceAdapter;

namespace NNTP_NEWS_CLIENT.Presentation.ViewModel;

public class SettingsViewModel : ViewModelBase
{
    
    
    private string _userName = "elithe01@easv365.dk";
    public string Username 
    {
        get { return _userName; }
        set { _userName = value; 
            OnPropertyChanged(); }
    }

    private string _password = "cf6290";
    public string Password
    {
        get { return _password; }
        set { _password = value; 
            OnPropertyChanged(); }
    }

    private string _host = "news.sunsite.dk";
    public string Host 
    {
        get { return _host; }
        set
        {
            _host = value;
            OnPropertyChanged(); }
    }
    private int _port = 119;
    public int Port
    {
        get { return _port; }
        set
        {
            _port = value;
            OnPropertyChanged();
        }
    }
    
    private string _newsGroup = "dk.test"; 
    public string NewsGroup 
    {
        get { return _newsGroup; }
        set {
            _newsGroup = value;
            OnPropertyChanged();
        }  
    }
    
    private bool IsUsernameValid(string userName)
    {
        return !string.IsNullOrEmpty(userName);
    }
    private bool IsPasswordValid(string password)
    {
        return !string.IsNullOrEmpty(password);
    }
    private bool IsHostValid(string host)
    {
        return !string.IsNullOrEmpty(host);
    }
    private bool IsPortValid(int port)
    {
        return (port > 0);
    }
    private bool IsNewsGroupValid(string newsGroup)
    {
        return !string.IsNullOrEmpty(newsGroup);
    }

    public ICommand ChangeToBrowserViewCommand => new CommandBase(ChangeToBrowserView);

    private void ChangeToBrowserView(object obj)
    {
        ViewModelController.Instance.SetCurrentViewModel(typeof(BrowserViewModel));
    }

    public ICommand LoginCommand => new CommandBase(EstablishClient, CanEstablishClient);
    private async void EstablishClient(object o)
    {
        
        // saving session data for later use.
        ViewModelController.Instance.AddSesionDate("USERNAME", Username);
        ViewModelController.Instance.AddSesionDate("PASSWORD", Password);
        ViewModelController.Instance.AddSesionDate("PORT", Port);
        ViewModelController.Instance.AddSesionDate("HOST", Host);
        ViewModelController.Instance.AddSesionDate("GROUP", NewsGroup);
        
        // establishing connection
        var establishConnectionUc = new EstablishConnectionUC(); 
        var response = await establishConnectionUc.ConnectAsync(Host, Port);

        if (response.ResponseCode != 200)
        {
            MessageBox.Show("Unable to establish connection to the host. Please try again.");
            return;
        }
        
        // authenticating the client. 
        var authenticated = await establishConnectionUc.AuthenticateUserAsync(Username, Password);
        if (authenticated.ResponseCode != 281) // 281, login success
        {
            MessageBox.Show("Authentication failed. Please try again.");
            return;
        } 
        
        // the authentication was successful. storing reffrence to client for later use.
        IClient client = establishConnectionUc.Client;
        ViewModelController.Instance.AddSesionDate("CLIENT", client);
        
        
        // change view 
        new BrowserViewModel();
        ViewModelController.Instance.SetCurrentViewModel(typeof(BrowserViewModel));
        
    }
    private bool CanEstablishClient(object o)
    {
        return (
            IsHostValid(Host) && 
            IsPasswordValid(Password) &&
            IsUsernameValid(Username) && 
            IsPortValid(Port) && 
            IsNewsGroupValid(NewsGroup));
    }
    
   
}