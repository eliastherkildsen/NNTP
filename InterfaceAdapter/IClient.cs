using NNTP_NEWS_CLIENT.Entitys;

namespace NNTP_NEWS_CLIENT.InterfaceAdapter;

public interface IClient
{
    Task<NntpRespons> ConnectAsync(string address, int port);
    Task<NntpRespons> SendAsync(string command);
    int Disconnect();
}