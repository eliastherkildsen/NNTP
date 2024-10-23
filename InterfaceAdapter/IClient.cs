namespace WPF_MVVM_TEMPLATE.InterfaceAdapter;

public interface IClient
{
    int Connect(string address, int port);
    int Send(string command);
    int Disconnect();
}