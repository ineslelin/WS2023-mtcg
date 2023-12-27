using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Models;
using ws2023_mtcg.Server.Repository;
using ws2023_mtcg.Server.Res;
using ws2023_mtcg.FightLogic.Enums;

namespace ws2023_mtcg.Server.Req
{
    internal class GetRequestHandler
    {
        string req;
        string data;

        TcpClient client;
        StreamReader reader;
        StreamWriter writer;

        public GetRequestHandler(TcpClient client, StreamReader reader, StreamWriter writer, string req) 
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

            if (route[1] == "/cards")
            {
                HandleCardsRequest();
            }
        }

        public void HandleCardsRequest()
        {
            try
            {
                TokenValidator.CheckTokenExistence(req);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "No token.");

                return;
            }

            string authHeader = "";

            try
            {
                authHeader = TokenValidator.GetAuthHeader(req);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Wrong token.");

                return;
            }

            string username = "";

            try
            {
                username = TokenValidator.SplitToken(authHeader);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Wrong token.");

                return;
            }

            StackRepository stackRepository = new StackRepository();
            List<Cards> stack = new List<Cards>();

            try
            {
                stack = stackRepository.Read(username).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "User doesn't have cards.");

                return;
            }

            string stackCards = "";

            foreach(var s in stack)
            {
                stackCards += $"ID: {s.Id}\n" +
                              $"Name: {s.Name}\n" +
                              $"Damage: {s.Damage}\n" +
                              $"Element: {s.Element}\n" +
                              $"Type: {(s.Type == CardType.monster ? "Monster" : "Spell")}\n\n";
            }

            ResponseHandler.SendResponse(writer, stackCards);
        }
    }
}
