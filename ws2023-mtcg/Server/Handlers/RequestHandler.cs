using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Server.Handlers
{
    internal class RequestHandler
    {
        public RequestHandler()
        {
            
        }

        public void HandleRequest(TcpClient client, string req)
        {
            string[] reqLines = req.Split('\n');

            string method = reqLines[0];
            // method looks like this: METHOD ROUTE HTTPSMTHSMTH => get route out of method, but how

            // Console.WriteLine(method);

            if (method.Contains("GET"))
                HandleGetRequest();

            if (method.Contains("POST"))
                HandlePostRequest();

            if (method.Contains("PUT"))
                HandlePutRequest();

            if (method.Contains("DELETE"))
                HandleDeleteRequest();
        }

        private void HandleGetRequest()
        {

        }

        private void HandlePostRequest()
        {

        }

        private void HandlePutRequest()
        {

        }

        private void HandleDeleteRequest()
        {

        }

    }
}
