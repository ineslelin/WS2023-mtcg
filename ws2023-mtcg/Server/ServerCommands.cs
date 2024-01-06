using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Server
{
    internal class ServerCommands
    {
        public TcpClient client;
        public StreamWriter writer;
        public StreamReader reader;

        public ServerCommands(TcpClient client, StreamWriter writer, StreamReader reader)
        {
            this.client = client;
            this.writer = writer;
            this.reader = reader;
        }

        public string RetrieveData()
        {
            char[] clientBuffer = new char[client.ReceiveBufferSize];
            int bytesRead = reader.Read(clientBuffer, 0, client.ReceiveBufferSize);
            return new string(clientBuffer, 0, bytesRead);
        }
    }
}
