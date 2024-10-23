namespace WPF_MVVM_TEMPLATE.Entitys;

public class Article
{
    
    public int ArticleId { get; }
    public int Bytes { get; }
    public int Lines { get; }

    public string? Subject { get; }
    public string? From { get; }
    public string? Data { get; }
    public string? MessageId { get; }
    public string? References { get; }
    public string? OptionalHeaders { get; }

    public Article(int articleId, int bytes, int lines, string? subject = null,
        string? from = null, string? data = null, string? messageId = null,
        string? references = null, string? optionalHeaders = null)
    {
        
        ArticleId = articleId;
        Bytes = bytes;
        Lines = lines;
        
        if (subject != null) Subject = subject;
        if (from != null) From = from;
        if (data != null) Data = data;
        if (messageId != null) MessageId = messageId;
        if (references != null) References = references;
        if (optionalHeaders != null) OptionalHeaders = optionalHeaders;
        
    }
    
}