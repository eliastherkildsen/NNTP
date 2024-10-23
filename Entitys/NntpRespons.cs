using System.Collections;
using System.Text.Json.Nodes;

namespace NNTP_NEWS_CLIENT.Entitys;

public class NntpRespons
{
    
    public int ResponseCode { get; }
    public object? ResponseData { get; }

    public NntpRespons(int responseCode, object responseData)
    {
        ResponseCode = responseCode;
        ResponseData = responseData;
    }

    public NntpRespons(int responseCode)
    {
        ResponseCode = responseCode;
        ResponseData = null;
    }
    
}