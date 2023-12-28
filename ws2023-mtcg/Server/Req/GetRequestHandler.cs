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
using System.Text.RegularExpressions;
using System.Xml.Linq;

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

            if (route[1] == "/deck")
            {
                HandleDeckRequest();
            }

            if (Regex.IsMatch(route[1], @"/users/[a-zA-Z]*"))
            {
                HandleUserRequest(route[1]);
            }

            if (route[1] == "/stats")
            {
                HandleStatsRequest();
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
                ResponseHandler.SendErrorResponse(writer, "Wrong token.", 401);

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

            StackRepository stackRepository = new StackRepository();
            List<Cards> stack = new List<Cards>();

            try
            {
                stack = stackRepository.ReadByOwner(username).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Error reading stack.", 400);

                return;
            }

            if(!stack.Any())
            {
                ResponseHandler.SendResponse(writer, "User doesn't have any cards", 204);
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

            ResponseHandler.SendResponse(writer, stackCards, 200);
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
                ResponseHandler.SendErrorResponse(writer, "Wrong token.", 401);

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

            DeckRepository deckRepository = new DeckRepository();
            List<Cards> deck = new List<Cards>();

            try
            {
                deck = deckRepository.Read(username).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Error reading deck.", 400);

                return;
            }

            if (!deck.Any())
            {
                ResponseHandler.SendResponse(writer, "User doesn't have a configured deck", 204);
            }

            string deckCards = "";

            foreach (var d in deck)
            {
                deckCards += $"ID: {d.Id}\n" +
                              $"Name: {d.Name}\n" +
                              $"Damage: {d.Damage}\n" +
                              $"Element: {d.Element}\n" +
                              $"Type: {(d.Type == CardType.monster ? "Monster" : "Spell")}\n\n";
            }

            ResponseHandler.SendResponse(writer, deckCards, 200);
        }

        public void HandleUserRequest(string route)
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
                ResponseHandler.SendErrorResponse(writer, "Wrong token.", 401);

                return;
            }

            string username = "";

            try
            {
                username = TokenValidator.SplitToken(authHeader);

                if (!route.Contains(username))
                {
                    if(!TokenValidator.CheckAdminToken(authHeader))
                        throw new Exception();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Wrong token.", 401);

                return;
            }

            UserProfileRepository userProfileRepository = new UserProfileRepository();
            string profile = "";

            try
            {
                profile = userProfileRepository.Read(username);

                if (profile == null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "No user profile found.", 404);

                return;
            }

            ResponseHandler.SendResponse(writer, profile, 200);
        }

        public void HandleStatsRequest()
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
                ResponseHandler.SendErrorResponse(writer, "Wrong token.", 401);

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

            UserRepository userRepository = new UserRepository();
            User tempUser = new User();

            try
            {
                tempUser = userRepository.Read(username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Couldn't get user", 400);

                return;
            }

            string stats = $"ELO: {tempUser.Elo}";

            ResponseHandler.SendResponse(writer, stats, 200);
        }
    }
}
