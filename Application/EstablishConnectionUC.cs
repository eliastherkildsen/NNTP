using WPF_MVVM_TEMPLATE.Infrastructure;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Application;

public class EstablishConnectionUC
{

    public IClient Client;
    
    public int Connect(string address, int port)
    {
        Client = new NntpClient();
        return Client.Connect(address, port);
    }

    public int Disconnect()
    {
        return Client.Disconnect();
    }
    
    public int AuthenticateUser(string username, string password)
    {
        // USERNAME 
        var response = Client.Send("AUTHINFO USER " + username);
        if (response != 381) return response; // (381)PASS REQUIRED, otherwise an error orcured.
        response = Client.Send("AUTHINFO PASS " + password);
        return response;
    }
    
    
    
    
    
    
    
}