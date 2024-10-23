using System.Windows.Input;
using WPF_MVVM_TEMPLATE.Application;
using WPF_MVVM_TEMPLATE.Infrastructure;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Presentation.ViewModel;

public class BrowserViewModel : ViewModelBase
{
    
    private IClient _client;
    
    public ICommand EstablishConnectionCommand => new CommandBase(EstablishConnection, CanEstablishConnection);

    private bool CanEstablishConnection(object arg)
    {
        return HasSessionData();
    }

    private void EstablishConnection(object obj)
    {
        // Fetching sessiondata
        var username = FetchSessionData("USERNAME");
        var password = FetchSessionData("PASSWORD");
        var host  = FetchSessionData("HOST");
        var port = FetchSessionData("PORT");
        
        
        // crating the client.
        _client = new NntpClient();
        var establishConnectionUc = new EstablishConnectionUC();
        
        var response = establishConnectionUc.Connect((string) host, (int) port);
        if (response != 200)
        {
            Console.WriteLine($"Recived respons when trying create connection {response}");
            return;
        }
        
        _client = establishConnectionUc.Client;
        
        response = establishConnectionUc.AuthenticateUser((string)username, (string) password);
        if (response != 281) // (281) connection established
        {
            Console.WriteLine($"Recived respons when trying create connection {response}");
            return;
        }
        
        // storing the clint for later use. 
        
        Console.WriteLine($"Authenticated user {username.ToString()}");
        
    }

    private bool HasSessionData()
    {
        var sessionData = ViewModelController.Instance.SessionData;
        
        return (sessionData.ContainsKey("USERNAME") &&
                sessionData.ContainsKey("PASSWORD") &&
                sessionData.ContainsKey("PORT") &&
                sessionData.ContainsKey("HOST"));
    }
    private object FetchSessionData(string identifier)
    {
        try
        {
            return ViewModelController.Instance.GetSesionDate(identifier);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
}