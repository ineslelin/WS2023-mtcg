using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace ws2023_mtcg
{
    internal class HttpServer
    {
        public int Port;
        private HttpListener _listener;

        public HttpServer(int port) 
        {
            Port = port;
        }

        public void Start()
        {
            _listener = new HttpListener();

            _listener.Prefixes.Add($"http://localhost:{Port}/");
            _listener.Start();

            Console.WriteLine("Listening...");

            while(true)
            {
                HttpListenerContext context = _listener.GetContext();
                HttpListenerRequest req = context.Request;
                HttpListenerResponse res = context.Response;

                HandleRequest(req, res);
            }
        }

        public void Stop()
        {
            _listener.Stop();
            
            Console.WriteLine("Server stopped");
        }

        public void HandleRequest(HttpListenerRequest req, HttpListenerResponse res)
        {
            
        }
    }
}
