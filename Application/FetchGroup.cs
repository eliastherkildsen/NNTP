using System.Text;
using NNTP_NEWS_CLIENT.Entitys;
using NNTP_NEWS_CLIENT.InterfaceAdapter;
using WPF_MVVM_TEMPLATE.Entitys;

namespace NNTP_NEWS_CLIENT.Application;

public class FetchGroup
{

    private int _firstArticleID = -1; 
    private int _lastArticleID  = -1;
    private int _totalArticals  = -1; 
    private string _groupName   = string.Empty;

    public async Task<NntpRespons> FetchArticlesForGroupInfo(IClient client, string groupName)
    {
        if (!IsGroupNameValid(groupName)) return new NntpRespons(500);
        var response = await client.SendAsync("group " + groupName);
        
        // 211 Successful response to the GROUP command, indicating the estimated number of messages in the group (“n”), first and last article numbers (“f” and “l”) and group name (“s”).
        if (response.ResponseCode != 211)
        {
            Console.WriteLine($"Error on fetching articles for group {groupName}");
            return response;
        }
        Console.WriteLine($"Fetching articles for group {groupName}");
        
        // the respons data for a succesfull respons where a group exists is as follow.
        // [0] = Responscode
        // [1] = Total articals 
        // [2] = Id for first article 
        // [3] = Id for last article 
        
        var data   = response.ResponseData.ToString().Split(" ");
        _groupName       = data[0];
        _totalArticals   = int.Parse(data[1]);
        _firstArticleID  = int.Parse(data[2]);
        _lastArticleID   = int.Parse(data[3]);
        
        Console.WriteLine($"Fetching articles for group {groupName}");
        Console.WriteLine($"Total articals: {_totalArticals}");
        Console.WriteLine($"First article id: {_firstArticleID}");
        Console.WriteLine($"Last article id: {_lastArticleID}");
        
        return response;
        
    }
    private async Task<NntpRespons> GetAllArticlesForGroup(IClient client, string groupName)
    {
        // validating group name 
        if (!IsGroupNameValid(groupName))
        {
            Console.WriteLine($"Invalid group name");
            return new NntpRespons(500); 
        };
        
        // validating article ids not negativ. 
        if (_firstArticleID <= 0 || _lastArticleID <= 0)
        {
            Console.WriteLine($"Invalid article ID");
            return new NntpRespons(500);
        }
        
        // validating article ids. 
        if (_firstArticleID > _lastArticleID)
        {
            Console.WriteLine($"Firstarticl id is greater than lastArticleID");
            return new NntpRespons(500);
        }
        
        var response = await client.SendAsync($"XOVER {_firstArticleID}-{_lastArticleID} ");
        Console.WriteLine($"Response data type {response.ResponseData.GetType().Name}");
        Console.WriteLine(response.ResponseData.ToString());
        return response; 
    }
    
    private const int MinGroupNameLength = 3;
    private const int MaxGroupNameLength = 128;
    private bool IsGroupNameValid(string groupName)
    {
        
        // null, whitespace check 
        if (string.IsNullOrWhiteSpace(groupName))
        {
            Console.WriteLine("Group name is empty or null");
            return false;
        };
        
        // length check 
        if (groupName.Length < MinGroupNameLength || groupName.Length >= MaxGroupNameLength)
        {
            Console.WriteLine($"Group name must be between {MinGroupNameLength} and {MaxGroupNameLength}, but was {groupName.Length}");
            return false;
        }
        
        return true;
        
    }
    
    
    // Parse article information, and split into Article obj. 
    public async Task<List<Article>> GetArticlesForGroup(IClient client, string groupName)
    {
        List<Article> articles = new List<Article>();
        
        // fetching the articals. 
        var respons = await GetAllArticlesForGroup(client, groupName);
        
        // step 1. get articla data. 
        var articals = respons.ResponseData.ToString().Split("<Delimiter>");
        Console.WriteLine($"Found {articals.Length} articles");

        foreach (var artical in articals)
        {
            int articleID = FetchArticalID(artical);
        }
        
        
        return articles;
        
    }

    /// <summary>
    /// To fetch the artical id we need to read until the first space. 
    /// </summary>
    /// <param name="artical"></param>
    /// <returns></returns>
    public static int FetchArticalID(string artical)
    {
        StringBuilder stringBuilder = new StringBuilder(); 
        
        for (int i = 0; i < artical.Length; i++)
        {
            if (artical[i] == ' ') break;
            stringBuilder.Append(artical[i]);
        }   
        
        if (stringBuilder.Length == 0) return -1;
        
        // parse string to an int. 
        int articleID = int.Parse(stringBuilder.ToString());
        
        return articleID; 
    }
    
    
}