using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Json.Net;

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
            // handle GET, POST, PUT, DELETE
            string method = req.HttpMethod.ToString();

            switch(method)
            {
                case "GET":
                    HandleGetRequest(req, res);
                    break;
                case "POST":
                    HandlePostRequest(req, res);
                    break;
                case "PUT":
                    HandlePutRequest(req, res);
                    break;
                case "DELETE":
                    HandleDeleteRequest(req, res);
                    break;
                default:
                    res.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                    break;
            }

            res.Close();
        }

        private void HandleGetRequest(HttpListenerRequest req, HttpListenerResponse res)
        {
            
        }

        private void HandlePostRequest(HttpListenerRequest req, HttpListenerResponse res) 
        { 

        }

        private void HandlePutRequest(HttpListenerRequest req, HttpListenerResponse res)
        { 
        
        }

        private void HandleDeleteRequest(HttpListenerRequest req, HttpListenerResponse res)
        {

        }
    }
}
