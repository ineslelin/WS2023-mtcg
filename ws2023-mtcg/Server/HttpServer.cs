using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Json.Net;
using System.Net.Sockets;
using ws2023_mtcg.Server.Handlers;

namespace ws2023_mtcg.Server
{
    internal class HttpServer
    {
        private readonly int port;
        private readonly IPAddress host = IPAddress.Loopback;
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

            while (listening)
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

            string? requestToHandle;
            StringBuilder req = new StringBuilder();

            while ((requestToHandle = reader.ReadLine()) != null)
            {
                Console.WriteLine(requestToHandle);
                req.AppendLine(requestToHandle);

                if (string.IsNullOrEmpty(requestToHandle))
                    break;
            }

            Console.WriteLine("Handling request...");

            if(req.ToString().Contains("GET"))
            {
                GetRequestHandler getRequestHandler = new GetRequestHandler(req.ToString());
            }

            if(req.ToString().Contains("POST") || req.ToString().Contains("PUT"))
            {
                char[] clientBuffer = new char[client.ReceiveBufferSize];
                int bytesRead = reader.Read(clientBuffer, 0, client.ReceiveBufferSize);
                string data = new string(clientBuffer, 0, bytesRead);

                if (req.ToString().Contains("POST"))
                {
                    PostRequestHandler postRequestHandler = new PostRequestHandler(req.ToString(), data);
                }
            }

            client.Close();

            Console.WriteLine("Client disconnected");
        }

        public void Stop()
        {
            _listener.Stop();

            Console.WriteLine("Server stopped");
        }
    }
}
