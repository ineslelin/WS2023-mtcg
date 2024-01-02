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
using Newtonsoft.Json;
using System.Collections;

namespace ws2023_mtcg.Server.Req
{
    internal class GetRequestHandler
    {
        string req;
        string data;
        string response;

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

            if (route[1] == "/scoreboard")
            {
                HandleScoreboardRequest();
            }
        }

        public void HandleCardsRequest()
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

            StackRepository stackRepository = new StackRepository();
            List<Cards> stack = new List<Cards>();

            try
            {
                stack = stackRepository.ReadByOwner(username).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Couldn't read stack",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            if(!stack.Any())
            {
                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "User doesn't have any cards",
                });

                ResponseHandler.SendResponse(writer, response, (int)ResponseCode.NoContent);

                return;
            }

            List<Card> cards = new List<Card>();

            foreach(var s in stack)
            {
                Card card = new Card
                {
                    Id = s.Id,
                    Name = s.Name,
                    Damage = s.Damage
                };

                cards.Add(card);
            }

            response = JsonConvert.SerializeObject(new
            {
                status = "success",
                stack = cards
            });

            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.Success);
        }

        public void HandleDeckRequest()
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

            DeckRepository deckRepository = new DeckRepository();
            List<Cards> deck = new List<Cards>();

            try
            {
                deck = deckRepository.Read(username).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Couldn't read deck",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            if (!deck.Any())
            {
                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "The request was fine, but the deck doesn't have any cards",
                });

                ResponseHandler.SendResponse(writer, response, (int)ResponseCode.NoContent);

                return;
            }

            List<Card> cards = new List<Card>();

            foreach (var d in deck)
            {
                Card card = new Card
                {
                    Id = d.Id,
                    Name = d.Name,
                    Damage = d.Damage
                };

                cards.Add(card);
            }

            response = JsonConvert.SerializeObject(new
            {
                status = "success",
                deck = cards
            });

            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.Success);
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

            ResponseHandler.SendPlaintextResponse(writer, profile, 200);
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

            ResponseHandler.SendPlaintextResponse(writer, stats, 200);
        }

        public void HandleScoreboardRequest()
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

            UserRepository userRepository = new UserRepository();

            User[] users = userRepository.ReadAllByElo();
            string scoreboard = "";
            int rank = 0;

            foreach(var u in users)
            {
                rank++;
                scoreboard += $"{rank}: {u.Username} - ELO: {u.Elo}\n";
            }

            ResponseHandler.SendPlaintextResponse(writer, scoreboard, 200);
        }
    }
}
