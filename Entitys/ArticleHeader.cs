namespace NNTP_NEWS_CLIENT.Entitys;

public class ArticleHeader
{
    public string ArticleId { get; set; }
    public string Subject { get; set; }
    public string From { get; set; }
    public DateTime Date { get; set; }
    public string MessageId { get; set; }
    public int Size { get; set; }
    public int Lines { get; set; }
    public string Xref { get; set; }

    public override string ToString()
    {
        return $"ArticleId {ArticleId}, Subject {Subject}, From {From}, Date {Date}, MessageId {MessageId}, Size {Size}, Lines {Lines}";
    }
}
