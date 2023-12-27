using Newtonsoft.Json;
using Npgsql.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Models;
using ws2023_mtcg.Server.Repository;
using ws2023_mtcg.Server.Res;

namespace ws2023_mtcg.Server.Req
{
    internal class PutRequestHandler
    {
        string req;
        string data;

        TcpClient client;
        StreamReader reader;
        StreamWriter writer;

        public PutRequestHandler(TcpClient client, StreamReader reader, StreamWriter writer, string req)
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

            if (route[1] == "/deck")
            {
                data = serverCommands.RetrieveData();
                HandleDeckRequest();
            }
        }

        public void HandleDeckRequest() 
        {
            try
            {
                TokenValidator.CheckTokenExistence(req);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "No token.", 401);

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
                ResponseHandler.SendErrorResponse(writer, "Invalid token.", 401);

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
                ResponseHandler.SendErrorResponse(writer, "Wrong token.", 401);

                return;
            }

            List<Cards>? deck = new List<Cards>();
            List<string> cardIds = new List<string>();

            try
            {
                cardIds = JsonConvert.DeserializeObject<List<string>>(data);

                foreach(var id in cardIds)
                {
                    deck.Add(new Cards { Id = id,
                                         Owner = username });
                }

                if (deck == null)
                    return;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Invalid JSON", 400);

                return;
            }

            if(deck.Count() < 4 || deck.Count() > 4)
            {
                ResponseHandler.SendErrorResponse(writer, "You need exactly 4 cards to build a deck.", 400);
                return;
            }

            StackRepository stackRepository = new StackRepository();

            try
            {
                for (int i = 0; i < deck.Count; i++)
                {
                    deck[i] = stackRepository.ReadById(deck[i].Id);

                    if (deck[i].Owner != username)
                        throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "User doesn't own cards with these ids.", 403);

                return;
            }

            DeckRepository deckRepository = new DeckRepository();

            try
            {
                foreach(var d in deck)
                {
                    deckRepository.Create(d);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Couldn't add cards to deck", 400);

                return;
            }

            ResponseHandler.SendResponse(writer, "Configured deck successfully.", 200);
        }
    }
}
