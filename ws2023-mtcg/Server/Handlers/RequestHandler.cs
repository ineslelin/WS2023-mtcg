// using Json.Net;
using Newtonsoft.Json;
// using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Objects;
using ws2023_mtcg.Server.Repository;

namespace ws2023_mtcg.Server.Handlers
{
    internal class RequestHandler
    {
        public RequestHandler()
        {
            
        }

        public void HandleRequest(TcpClient client, StringBuilder req)
        {
            ServerCommands serverCommands = new ServerCommands();
            //string[] reqLines = req.Split('\n');

            //string method = reqLines[0];
            //// method looks like this: METHOD ROUTE HTTPSMTHSMTH => get route out of method, but how

            //// Console.WriteLine(method);

            if (req.ToString().StartsWith("GET"))
            {
                HandleGetRequest(req.ToString());
            }

            if (req.ToString().StartsWith("POST"))
            {
                int contentLength = ServerCommands.GetContentLength(req.ToString());
                string body = serverCommands.ReadRequestBody(client, contentLength);

                HandlePostRequest(req.ToString(), body);
            }

            if (req.ToString().StartsWith("PUT"))
            {
                HandlePutRequest(req.ToString());
            }

            if (req.ToString().StartsWith("DELETE"))
            {
                HandleDeleteRequest(req.ToString());
            }
        }

        private void HandleGetRequest(string route)
        {
            
        }

        private void HandlePostRequest(string route, string body)
        {
            if (route.Contains("users"))
            {
                User? tempUser = JsonConvert.DeserializeObject<User>(body);

                Console.WriteLine($"username {tempUser.Username}, password {tempUser.Password}");

               // User user = UserRepository.Create(tempUser);
            }
        }

        private void HandlePutRequest(string route)
        {

        }

        private void HandleDeleteRequest(string route)
        {

        }

    }
}
