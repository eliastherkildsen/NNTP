namespace NNTP_NEWS_CLIENT.Entitys;

public class Article
{
    
    public int ArticleId { get; }
    public int? Bytes { get; }
    public int? Lines { get; }

    public string? Subject { get; }
    public string? From { get; }
    public string? Date { get; }
    public string? MessageId { get; }
    public string? References { get; }
    public string? OptionalHeaders { get; }

    public Article(int articleId, int? bytes = null, int? lines = null, string? subject = null,
        string? from = null, string? date = null, string? messageId = null,
        string? references = null, string? optionalHeaders = null)
    {
        
        ArticleId = articleId;
        
        if (lines != null) Lines = lines;
        if (Bytes != null) Bytes = bytes;
        if (subject != null) Subject = subject;
        if (from != null) From = from;
        if (date != null) Date = date;
        if (messageId != null) MessageId = messageId;
        if (references != null) References = references;
        if (optionalHeaders != null) OptionalHeaders = optionalHeaders;
        
    }
    
    public override string ToString()
    {
        return $"ArticleId {ArticleId}, Subject {Subject}, From {From}, Date {Date}, MessageId {MessageId}, Size {Bytes}, Lines {Lines}";
    }
    
}