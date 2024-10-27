using System.Windows.Input;

namespace NNTP_NEWS_CLIENT.Presentation.ViewModel;

public class ArticleViewModel : ViewModelBase
{
    public string ArticleId { get; set; } = string.Empty;
    public string ArticleSubject { get; set; } = string.Empty;
    public string ArticleBody { get; set; } = string.Empty;
    public ICommand ReturnToBrowserCommand = new CommandBase(ReturnToBrowser);

    private static void ReturnToBrowser(object obj)
    {
        ViewModelController.Instance.SetCurrentViewModel(typeof(BrowserViewModel));
    }
    
    
}