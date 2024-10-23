using System.Windows.Input;

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
    
    
    public SettingsViewModel()
    {
        ViewModelController.Instance.SetCurrentViewModel(typeof(SettingsViewModel));
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
    private void EstablishClient(object o)
    {
        
        ViewModelController.Instance.AddSesionDate("USERNAME", Username);
        ViewModelController.Instance.AddSesionDate("PASSWORD", Password);
        ViewModelController.Instance.AddSesionDate("PORT", Port);
        ViewModelController.Instance.AddSesionDate("HOST", Host);
        ViewModelController.Instance.AddSesionDate("GROUP", NewsGroup);
        
        
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