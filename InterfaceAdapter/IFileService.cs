namespace NNTP_NEWS_CLIENT.InterfaceAdapter;

public interface IFileService
{
    bool IsDirectoryAvailable(string directory);
    bool DirectoryContainsFiles(string directory); 
    List<string> GetFiles(string directory);
    bool FileIsOfType(string filename, string type);
}