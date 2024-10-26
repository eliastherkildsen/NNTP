using NNTP_NEWS_CLIENT.Entitys;
using WPF_MVVM_TEMPLATE.Entitys;

namespace NNTP_NEWS_CLIENT.InterfaceAdapter;

public interface IXOverXConverter
{
    Article DecodeArticle(int index);
    List<Article> FetchAllArticles();
}