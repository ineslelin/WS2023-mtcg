using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ws2023_mtcg.Server.Res;

namespace ws2023_mtcg.Server.Req
{
    internal class DeleteRequestHandler
    {
        string req;
        string data;
        string response;

        TcpClient client;
        StreamReader reader;
        StreamWriter writer;

        public DeleteRequestHandler(TcpClient client, StreamReader reader, StreamWriter writer, string req)
        {
            if (req == null)
                throw new ArgumentNullException();

            this.client = client;
            this.reader = reader;
            this.writer = writer;
            ServerCommands serverCommands = new ServerCommands(client, writer, reader);

            this.req = req;
            this.data = "";

            string[] reqLines = req.Split("\n");
            string[] route = reqLines[0].Split(" ");

            if (Regex.IsMatch(route[1], @"/tradings/[a-zA-Z0-9-]*"))
            {
                string[] id = route[1].Split("/");
                HandleTradingRequest(id[1]);
            }
        }

        public void HandleTradingRequest(string id)
        {
            string username = "";

            try
            {
                TokenValidator.CheckTokenExistence(req);
                string authHeader = TokenValidator.GetAuthHeader(req);
                username = TokenValidator.SplitToken(authHeader);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Access token is missing or invalid",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Unauthorized);

                return;
            }


        }
    }
}
