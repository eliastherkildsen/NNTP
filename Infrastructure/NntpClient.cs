using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WPF_MVVM_TEMPLATE.InterfaceAdapter;

namespace WPF_MVVM_TEMPLATE.Infrastructure
{
    public class NntpClient : IClient
    {
       
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;
        private Encoding _encoding = Encoding.ASCII;
        
        public int Connect(string address, int port)
        {
            return SetupTcpClient(address, port);
        }

        public int Disconnect()
        {
            _streamWriter?.Close();
            _streamReader?.Close();
            _streamWriter?.Dispose();
            _streamReader?.Dispose();
            _tcpClient?.Close();
            _tcpClient?.Dispose();
            
            return 200;
        }

        public int Send(string command)
        {
            
            // validating client
            if (_tcpClient == null || !_tcpClient.Connected)
            {
                Console.WriteLine("The client is not connected.");
                return -1;
            }
            
            // validating the streamwriter. 
            if (_streamWriter == null)
            {
                Console.WriteLine("The stream writer is not initialized.");
                return -1; 
            }
            
            // validating streamreader 
            if (_streamReader == null)
            {
                Console.WriteLine("The stream reader is not initialized.");
                return -1;
            }
            
            // validating command. 
            if (command == null || command.Length == 0)
            {
                Console.WriteLine("The command is empty.");
                return -1;
            }
            
            _streamWriter.WriteLine(command);
            string response = _streamReader.ReadLine();
            
            // informing user.
            Console.WriteLine($"Got this response from send message. {response}");

            if (response != null) return FetchResponseCode(response);
            return -1;
        }
        
        private int SetupTcpClient(string address, int port)
        {
            
            try
            {
                // setting up the tcpclient. 
                _tcpClient = new TcpClient();
                _tcpClient.Connect(address, port);
                
                // setting up streams 
                _networkStream = _tcpClient.GetStream();
                _streamReader = new StreamReader(_networkStream, _encoding);
                _streamWriter = new StreamWriter(_networkStream, _encoding);
                _streamWriter.AutoFlush = true;
                
                // read respons from tcp client. 
                var response = _streamReader.ReadLine();
                var responsCode = FetchResponseCode(response);
                
                Console.WriteLine($"Connected to {address}:{port} with response code {responsCode}");
                return responsCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
            
        }

        private int FetchResponseCode(string response)
        {
            if (response.Length < 3) throw new Exception("Invalid response from server.");

            string responseCode = response.Substring(0, 3);
            if (!int.TryParse(responseCode, out int code))
            {
                throw new Exception("Failed to parse response code.");
            }
            return code;
        }
        
        
        
    }
}
