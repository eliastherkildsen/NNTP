using NNTP_NEWS_CLIENT.Entitys;
using NNTP_NEWS_CLIENT.Infrastructure;
using NNTP_NEWS_CLIENT.InterfaceAdapter;

namespace NNTP_NEWS_CLIENT.Application;

public class LoadArticle
{
    
    private IClient _client;
    private Group   _group;
    
    public LoadArticle(IClient client)
    {
        _client = client;
    }
    
    // Parse article information, and split into Article obj. 
    public async Task<List<Article>> GetArticlesForGroup()
    {
        List<Article> articles = new List<Article>();
        
        // fetching the articals. 
        var respons = await GetAllArticlesForGroup(_client, _group);
        
        // step 1. get articla data. 
        var loadedArticalString = respons.ResponseData.ToString();
        
        //Console.WriteLine($"Found {loadedArticalString.Length} articles");

        IXOverXConverter xConverter = new XOverDecoder(loadedArticalString);

        articles = xConverter.FetchAllArticles();
        
        //Console.WriteLine($"Returned {articles.Count} articles");
        
        return articles;
        
    }
    
    public async Task<NntpRespons> GetAllArticlesForGroup(IClient client, Group group)
    {
        
        // validating article ids not negativ. 
        if (group.FirstArticalIndex <= 0 || group.LastArticalIndex <= 0)
        {
            Console.WriteLine($"Invalid article ID");
            return new NntpRespons(500);
        }
        
        // validating article ids. 
        if (group.FirstArticalIndex > group.LastArticalIndex)
        {
            Console.WriteLine($"First article id is greater than lastArticleID");
            return new NntpRespons(500);
        }
        
        var response = await client.SendAsync($"XOVER {group.FirstArticalIndex}-{group.LastArticalIndex}"); //{_firstArticleID}-{_lastArticleID}
        Console.WriteLine($"Response data type {response.ResponseData.GetType().Name}");
        Console.WriteLine(response.ResponseData.ToString());
        return response; 
    }
    
}