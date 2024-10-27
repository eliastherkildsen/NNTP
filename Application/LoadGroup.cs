using System.Text;
using NNTP_NEWS_CLIENT.Entitys;
using NNTP_NEWS_CLIENT.Infrastructure;
using NNTP_NEWS_CLIENT.InterfaceAdapter;
using WPF_MVVM_TEMPLATE.Entitys;

namespace NNTP_NEWS_CLIENT.Application;

public class LoadGroup
{
    private Group _group;
    private string _groupName;
    private IClient _client;
    public LoadGroup(IClient client, string groupName)
    {
        _client = client;
        _groupName = groupName;
    }
    
    public async Task<Group> FetchGroupInfo()
    {
        if (!IsGroupNameValid(_groupName)) return new Group { GroupName = _groupName };
        var response = await _client.SendAsync("group " + _groupName);
        
        // 211 Successful response to the GROUP command, indicating the estimated number of messages in the group (“n”), first and last article numbers (“f” and “l”) and group name (“s”).
        if (response.ResponseCode != 211)
        {
            Console.WriteLine($"Error on fetching articles for group {_groupName}");
            return new Group { GroupName = _groupName };
        }
        
        var data   = response.ResponseData.ToString().Split(" ");
        _groupName = data[0];
        int _totalArticals   = int.Parse(data[1]);
        int _firstArticleID  = int.Parse(data[2]);
        int _lastArticleID   = int.Parse(data[3]);
        
        _group = new Group{ GroupName = _groupName, TotalArticals = _totalArticals, FirstArticalIndex = _firstArticleID, LastArticalIndex = _lastArticleID };
        return new Group{ GroupName = _groupName, TotalArticals = _totalArticals, FirstArticalIndex = _firstArticleID, LastArticalIndex = _lastArticleID };
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
    
    
   
    
}