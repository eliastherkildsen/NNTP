using NNTP_NEWS_CLIENT.Entitys;
using NNTP_NEWS_CLIENT.InterfaceAdapter;
using WPF_MVVM_TEMPLATE.Entitys;

namespace NNTP_NEWS_CLIENT.Infrastructure;

public class XOverDecoder : IXOverXConverter
{

    private string _fileData;
    private string filePath; 
    private List<string> _dataObjects;
    private string _delimiter = "<Delimiter>";
    
    public XOverDecoder(string data)
    {
        _fileData = data;
        FetchDataElements();
    }
    
    public Article DecodeArticle(int index)
    {
        
        if (index < 0 || index >= _dataObjects.Count) throw new IndexOutOfRangeException();
        if (_dataObjects[index] is null) throw new Exception("Article not found or null");

        int articleId = FetchArticleId(index);
        string subject = FetchArticleSubject(index);
        //Console.WriteLine($"Fetched article with id: {articleId}");
        
        return new Article(articleId:articleId, subject:subject);
    }
    public int FetchDataElements()
    {
        _dataObjects = new List<string>();
        var elements = _fileData.Split(_delimiter);
        foreach (var entry in elements)
        {
            _dataObjects.Add(entry);
        }
        
        return _dataObjects.Count;
        
    }

    public int FetchArticleId(int index)
    {
        if (index < 0 || index >= _dataObjects.Count) throw new IndexOutOfRangeException("The index is out of range. Artical does not exist.");

        char delimiter = (char) 9;
        string artical = _dataObjects[index];
        string id = artical.Split(delimiter).First();
        
        // checking if value corresponds to an integer. 
        if (id is null) throw new Exception($"ArticalID not found, recived NULL when fetching the article ID.");
        
        
        //Console.WriteLine($"The id is: {id}");
        
        
        if (int.TryParse(id, out int result)) return result;
        throw new Exception($"ArticalID is not a numaric value");
        
    }

    public string FetchArticleSubject(int index)
    {
        if (index <= 0 || index >= _dataObjects.Count) throw new IndexOutOfRangeException();
        if (_dataObjects[index] is null) throw new Exception("Article not found or null");
        
        char delimiter = (char) 9;
        string artical = _dataObjects[index];
        var recived = artical.Split(delimiter);
        if (recived.Length >= 2) return recived[1];
        throw new Exception("Artical subject not found");
    }

    public List<Article> FetchAllArticles()
    {
        List<Article> articles = new List<Article>();

        for (int i = 1; i < _dataObjects.Count; i++)
        {
            try
            {
                Article article = DecodeArticle(i);
                articles.Add(article);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
        
        return articles;
        
    }
}