using NNTP_NEWS_CLIENT.Entitys;
using NNTP_NEWS_CLIENT.Infrastructure;
using NNTP_NEWS_CLIENT.InterfaceAdapter;

namespace NNTP_NEWS_CLIENT.Application;

public class EstablishConnectionUC
{

    public IClient Client;

    public EstablishConnectionUC(IClient client)
    {
        Client = client;
    }
    
    public async Task<NntpRespons> ConnectAsync(string address, int port)
    {
        Client = new NntpClient();
        var response = await Client.ConnectAsync(address, port);
        return response; 

    }

    public int Disconnect()
    {
        return Client.Disconnect();
    }
    
    
    public async Task<NntpRespons> AuthenticateUserAsync(string username, string password)
    {
        try
        {
            // Send the username command
            var response = await Client.SendAsync($"AUTHINFO USER {username}");
        
            // Check if the response indicates that the password is required (381)
            if (response.ResponseCode != 381)
            {
                // If not 381, return the response directly as an error or success code
                return response;
            }

            // If 381, send the password
            response = await Client.SendAsync($"AUTHINFO PASS {password}");
        
            // Return the final response (whether success or failure)
            return response;
        }
        catch (Exception ex)
        {
            // Handle and log the exception if something goes wrong
            Console.WriteLine($"Authentication failed: {ex.Message}");
            return new NntpRespons(500); // or some other code indicating an error
        }
    }

    
    
    
    
    
    
}