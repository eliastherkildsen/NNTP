using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;
using NNTP_NEWS_CLIENT.Entitys;
using NNTP_NEWS_CLIENT.InterfaceAdapter;

namespace NNTP_NEWS_CLIENT.Infrastructure
{
    public class NntpClient : IClient
    {
       
        private TcpClient      _tcpClient;
        private NetworkStream  _networkStream;
        private StreamReader   _streamReader;
        private StreamWriter   _streamWriter;
        private Encoding       _encoding = Encoding.ASCII;
        
        public int Disconnect()
        {

            try
            {
                _streamWriter?.Close();
                _streamReader?.Close();
                _streamWriter?.Dispose();
                _streamReader?.Dispose();
                _tcpClient?.Close();
                _tcpClient?.Dispose();
                return 200;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1; 
            }
            
            
        }

        public async Task<NntpRespons> SendAsync(string command)
        {
            // Validating client
            if (_tcpClient == null || !_tcpClient.Connected)
            {
                Console.WriteLine("The client is not connected.");
                return new NntpRespons(500);
            }

            // Validating streamwriter
            if (_streamWriter == null)
            {
                Console.WriteLine("The stream writer is not initialized.");
                return new NntpRespons(500);
            }

            // Validating streamreader
            if (_streamReader == null)
            {
                Console.WriteLine("The stream reader is not initialized.");
                return new NntpRespons(500);
            }

            // Validating command
            if (string.IsNullOrEmpty(command))
            {
                Console.WriteLine("The command is empty.");
                return new NntpRespons(500);
            }
            
            

            // Send the command to the server
            await _streamWriter.WriteLineAsync(command);
            
            // Read the first line (response code and possibly part of the data)
            var response = await _streamReader.ReadLineAsync();
            if (response == null)
            {
                return new NntpRespons(500); // Error handling: No response received
            }

            // Fetch and validate the response code
            int responseCode = FetchResponseCode(response);
            string Delimiter = "<Delimiter>"; 
            // Initialize a StringBuilder to accumulate multi-line responses
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(response + Delimiter);
            
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append(response + Delimiter);
            
            // Check if the response indicates that additional data follows
            if (responseCode != 224)
            {
                return new NntpRespons(responseCode, stringBuilder.ToString());
            }

            // Read additional lines if this is a multi-line response
            string line;
            while ((line = await _streamReader.ReadLineAsync()) != null)
            {
                if (line == ".") // End of multi-line response
                {
                    break;
                }
                stringBuilder.Append(line + Delimiter);
                //stringBuilder2.Append(line + Delimiter);
            }
            
            WriteToASCIIFile(stringBuilder2);
            
            // Return the complete response
            return new NntpRespons(responseCode, stringBuilder.ToString());
        }
        
        public async Task<NntpRespons> ConnectAsync(string address, int port)
        {
            
            try
            {
                // setting up the tcpclient. 
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(address, port);
                
                // setting up streams 
                _networkStream = _tcpClient.GetStream();
                _streamReader = new StreamReader(_networkStream, _encoding);
                _streamWriter = new StreamWriter(_networkStream, _encoding);
                _streamWriter.AutoFlush = true;
                
                // read respons from tcp client. 
                var response = await _streamReader.ReadLineAsync();
                
                // validating respons. 
                if (response != null)
                {
                    var responsCode = FetchResponseCode(response);
                    Console.WriteLine($"Connected to {address}:{port} with response code {responsCode}");
                    return new NntpRespons(responsCode);
                }
                
                return new NntpRespons(500);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new NntpRespons(500);
            }
            
        }

        public static int FetchResponseCode(string response)
        {
            // null check
            if (response == null) return 500; 
            
            // min length check 
            if (response.Length < 3) return 500;

            string responseCode = response.Substring(0, 3);
            if (!int.TryParse(responseCode, out int code))
            {
                return 500; 
            }
            return code;
        }

        private void WriteToASCIIFile(StringBuilder stringBuilder)
        {
            // Specify the file path
            string filePath = "TestData.txt";

            // Write ASCII values to a file
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.ASCII))
            {
                writer.Write(stringBuilder.ToString());
            }

            Console.WriteLine("ASCII values written to file: " + filePath);
        }
        
        
        
    }
}
