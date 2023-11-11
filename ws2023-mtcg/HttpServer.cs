using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Json.Net;
using System.Net.Sockets;

namespace ws2023_mtcg
{
    internal class HttpServer
    {
        private int port;
        IPAddress host = IPAddress.Loopback;
        private TcpListener _listener;

        bool listening;

        public HttpServer(int clientPort) 
        {
            port = clientPort;
            listening = true;
        }

        public void Start()
        {
            _listener = new TcpListener(host, port);
            _listener.Start();

            Console.WriteLine("Server started...");

            byte[] buffer = new byte[1024];

            while(listening)
            {
                Console.WriteLine("Waiting for incoming connections...");

                TcpClient client = _listener.AcceptTcpClient();
                new Thread(() => HandleClient(client, buffer)).Start();
            }
        }

        public void HandleClient(TcpClient client, byte[] buffer)
        {
            Console.WriteLine("Accepted new client connection...");

            using var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            using var reader = new StreamReader(client.GetStream());

            string? req;

            while((req = reader.ReadLine()) != null)
            {
                Console.WriteLine(req); 
                Console.WriteLine("\r\n");
            }

            Console.WriteLine("Client disconnected");
        }

        public void Stop()
        {
            _listener.Stop();
            
            Console.WriteLine("Server stopped");
        }
    }
}
