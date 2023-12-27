using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Json.Net;
using System.Net.Sockets;
using ws2023_mtcg.Server.Req;
using ws2023_mtcg.Server.Res;

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

            ServerCommands serverCommands = new ServerCommands(client, writer, reader);

            if (req.ToString().Contains("GET"))
            {
                GetRequestHandler getRequestHandler = new GetRequestHandler(client, reader, writer, req.ToString());
            }

            if (req.ToString().Contains("POST"))
            {
                PostRequestHandler postRequestHandler = new PostRequestHandler(client, reader, writer, req.ToString());
            }

            if (req.ToString().Contains("PUT"))
            {
                PutRequestHandler putRequestHandler = new PutRequestHandler(client, reader, writer, req.ToString());
            }

            if (req.ToString().Contains("DELETE"))
            {
                DeleteRequestHandler deleteRequestHandler = new DeleteRequestHandler();
            }

            // ResponseHandler.SendResponse(writer, data);

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
