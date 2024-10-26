using System.Collections.ObjectModel;
using System.Windows.Input;
using NNTP_NEWS_CLIENT.Application;
using NNTP_NEWS_CLIENT.Entitys;
using NNTP_NEWS_CLIENT.Infrastructure;
using NNTP_NEWS_CLIENT.InterfaceAdapter;
using WPF_MVVM_TEMPLATE.Entitys;

namespace NNTP_NEWS_CLIENT.Presentation.ViewModel;

public class BrowserViewModel : ViewModelBase
{
    
    private IClient _client;
    private ObservableCollection<Article> _articleList = new ObservableCollection<Article>();

    public ObservableCollection<Article> ArticleList
    {
        get { return _articleList; }
        set {
            _articleList = value; 
            OnPropertyChanged();
            
        }
    }
    
    
    
    #region Fetch group
    public ICommand FetchGroupCommand => new CommandBase(FetchGroup);

    private async void FetchGroup(object obj)
    {
        var groupName = (string)ViewModelController.Instance.GetSesionDate("GROUP"); 
        var loadGroup = new LoadGroup(_client, groupName);
        var group = await loadGroup.FetchGroupInfo(_client, groupName);
        
        var loadArticle = new LoadArticle(_client);
        loadArticle.GetArticlesForGroup();
        var articles = await loadArticle.GetArticlesForGroup();
        
        Console.WriteLine("Fetch command called!");
        Console.WriteLine($"{articles.Count} articles found when adding to obs. list");
        
        foreach (var artical in articles)
        {
            //Console.WriteLine("Added Artical to obs. list");
            ArticleList.Add(artical);
        }
        
    }

    #endregion
    
    # region Establish Connection 
    public ICommand EstablishConnectionCommand => new CommandBase(EstablishConnection, CanEstablishConnection);

    private bool CanEstablishConnection(object arg)
    {
        return HasSessionData();
    }
    private async void EstablishConnection(object obj)
    {
        // Fetching sessiondata
        var username = FetchSessionData("USERNAME");
        var password = FetchSessionData("PASSWORD");
        var host  = FetchSessionData("HOST");
        var port = FetchSessionData("PORT");
        
        
        // crating the client.
        _client = new NntpClient();
        var establishConnectionUc = new EstablishConnectionUC(_client);
        
        var response = await establishConnectionUc.ConnectAsync((string) host, (int) port);
        if (response.ResponseCode != 200)
        {
            Console.WriteLine($"Recived respons when trying create connection {response}");
            return;
        }
        
        _client = establishConnectionUc.Client;


        response = await establishConnectionUc.AuthenticateUserAsync((string)username, (string)password); 
        
        if (response.ResponseCode != 281) // (281) connection established
        {
            Console.WriteLine($"Recived respons when trying create connection {response}");
            return;
        }
        
        // storing the clint for later use. 
        
        Console.WriteLine($"Authenticated user {username.ToString()}, client ready to use");
        
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