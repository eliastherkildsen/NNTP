using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NNTP_NEWS_CLIENT.Application;
using NNTP_NEWS_CLIENT.Entitys;
using NNTP_NEWS_CLIENT.Infrastructure;
using NNTP_NEWS_CLIENT.InterfaceAdapter;

namespace NNTP_NEWS_CLIENT.Presentation.ViewModel;

public class BrowserViewModel : ViewModelBase
{
    
    private IClient? _client;
    private Group? _group;
    public ObservableCollection<Article> Articles { get; }

    public BrowserViewModel()
    {
        Articles = new ObservableCollection<Article>();
        _client = (IClient) ViewModelController.Instance.GetSesionDate("CLIENT"); 
        SetupBrowser();
    }

    public void SortArticles()
    {
        
        
        
    }

    private async void SetupBrowser()
    {
        
        _group = await FetchGroup(); 
        if (_group == null)
        {
            MessageBox.Show("Failed to establish connection.");
            return;
        }

        var articlesForGroup = await LoadArticles();
        if (articlesForGroup == null)
        {
            MessageBox.Show("Failed to load articles.");
            return;
        }
        
        
        foreach (var article in articlesForGroup)
        {
            Articles.Add(article);
        }
    }
    
    #region Fetch group

    private async Task<Group> FetchGroup()
    {

        if (_client == null)
        {
            MessageBox.Show("You haven't connected yet. Please connect first.");
            return null;
        }
        
        var groupName = (string)ViewModelController.Instance.GetSesionDate("GROUP"); 
        var loadGroup = new LoadGroup(_client, groupName);
        var groupInformation = await loadGroup.FetchGroupInfo(); 
        return groupInformation;
    }

    private async Task<List<Article>> LoadArticles()
    {

        if (_group == null || _client == null)
        {
            MessageBox.Show("You haven't loaded group yet.");
            return null;
        }

        var loadArticle = new LoadArticle(_client, _group);
        var articlesForGroup = await loadArticle.GetArticlesForGroup();
        return articlesForGroup;
        

    }
    
    

    #endregion
    
    # region Establish Connection 
   
    private bool CanEstablishConnection()
    {
        return HasSessionData();
    }
    private async Task<IClient> EstablishConnection()
    {
        // Fetching sessiondata
        var username = FetchSessionData("USERNAME");
        var password = FetchSessionData("PASSWORD");
        var host  = FetchSessionData("HOST");
        var port = FetchSessionData("PORT");
        
        var establishConnectionUc = new EstablishConnectionUC();
        
        var response = await establishConnectionUc.ConnectAsync((string) host, (int) port);
        if (response.ResponseCode != 200)
        {
            Console.WriteLine($"Recived respons when trying create connection {response}");
            return null;
        }
        
        response = await establishConnectionUc.AuthenticateUserAsync((string)username, (string)password); 
        
        if (response.ResponseCode != 281) // (281) connection established
        {
            Console.WriteLine($"Recived respons when trying create connection {response}");
            return null;
        }
        
        // storing the clint for later use. 
        
        Console.WriteLine($"Authenticated user {username.ToString()}, client ready to use");
        return establishConnectionUc.Client;
        
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
    
    # endregion

}