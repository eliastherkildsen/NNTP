using NNTP_NEWS_CLIENT.Entitys;
using NNTP_NEWS_CLIENT.Infrastructure;
using NNTP_NEWS_CLIENT.InterfaceAdapter;

namespace NNTP_NEWS_CLIENT.Application;

public class LoadArticle
{
    private readonly IClient _client;
    private readonly Group _group;
    
    public LoadArticle(IClient client, Group group)
    {
        _client = client;
        _group = group;
    }
    
    // Parse article information, and split into Article obj. 
    public async Task<List<Article>> GetArticlesForGroup()
    {
        List<Article> articles = new List<Article>();
        
        // fetching the articles. 
        var allArticlesForGroup = await FetchArticlesFromServer(_client, _group);
        
        // validating response.
        if (allArticlesForGroup.ResponseCode == 500)
        {
            Console.WriteLine("Failed to get articles for group");
            return articles;
        }
        
        // validating that the response has respondent. 
        if (allArticlesForGroup.ResponseData == null)
        {
            Console.WriteLine("Failed to get articles for group. the response is empty");
            return articles;
        }
        
        var data = allArticlesForGroup.ResponseData.ToString();
        
        IXOverXConverter xConverter = new XOverDecoder(data);

        articles = xConverter.FetchAllArticles();
        
        //Console.WriteLine($"Returned {articles.Count} articles");
        
        return articles;
        
    }
    
    private async Task<NntpRespons> FetchArticlesFromServer(IClient client, Group group)
    {
        // validating group
        if (_group.FirstArticalIndex <= 0 || _group.LastArticalIndex <= 0)
        {
            Console.WriteLine($"Invalid article ID");
            return new NntpRespons(500);
        }
        
        // validating article ids. 
        if (_group.FirstArticalIndex > _group.LastArticalIndex)
        {
            Console.WriteLine($"First article id is greater than lastArticleID");
            return new NntpRespons(500);
        }
        
        var response = await client.SendAsync($"XOVER {group.FirstArticalIndex}-{group.LastArticalIndex}"); 
        return response; 
    }

    public async Task<Article> GetArticleById(string articleId)
    {
        var artical = await _client.SendAsync($"BODY {articleId}");
        
        // validating that an actual article has been received. 
        if (artical.ResponseCode != 221 || artical.ResponseData == null)
        {
            Console.WriteLine("Failed to get article");
            return null; 
        }
        
        var data = artical.ResponseData.ToString();
        Console.WriteLine($"Article ID: {articleId}");
        return null;
    }
}