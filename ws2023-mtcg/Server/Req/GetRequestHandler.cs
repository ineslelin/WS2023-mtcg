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

            if (route[1].Contains("/deck"))
            {
                HandleDeckRequest(route[1]);
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

            if (route[1] == "/tradings")
            {
                HandleTradingRequest();
            }
        }

        private void HandleCardsRequest()
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

            StackRepository stackRepository = new StackRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");
            List<Cards> stack = new List<Cards>();

            try
            {
                stack = stackRepository.Read(username).ToList();
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

        private void HandleDeckRequest(string route)
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

            DeckRepository deckRepository = new DeckRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");
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

            if (route == "/deck?format=plain")
            {
                foreach (var d in cards)
                {
                    response += $"ID: {d.Id}\n" +
                                $"Name: {d.Name}\n" +
                                $"Damage: {d.Damage}\n\n";
                }

                ResponseHandler.SendResponse(writer, response, (int)ResponseCode.Success);
            }

            response = JsonConvert.SerializeObject(new
            {
                status = "success",
                deck = cards
            });

            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.Success);
        }

        private void HandleUserRequest(string route)
        {
            string username = "";

            try
            {
                TokenValidator.CheckTokenExistence(req);
                string authHeader = TokenValidator.GetAuthHeader(req);
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

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Access token is missing or invalid",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Unauthorized);

                return;
            }

            UserProfileRepository userProfileRepository = new UserProfileRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");
            User profile = new User();

            try
            {
                profile = userProfileRepository.Read(username);

                if (profile == null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "User not found.",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.NotFound);

                return;
            }

            response = JsonConvert.SerializeObject(new
            {
                status = "success",
                user = new UserData
                {
                    Name = profile.Name,
                    Bio = profile.Bio,
                    Image = profile.Image,
                }
            });

            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.Success);
        }

        private void HandleStatsRequest()
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

            UserRepository userRepository = new UserRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");
            User tempUser = new User();

            try
            {
                tempUser = userRepository.Read(username);

                if (tempUser == null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "User not found.",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.NotFound);

                return;
            }

            response = JsonConvert.SerializeObject(new
            {
                status = "success",
                stats = new UserStats
                {
                    Name = tempUser.Username,
                    Elo = tempUser.Elo,
                    Wins = tempUser.Wins,
                    Losses = tempUser.Losses,
                },
                    winToLoseRatio = $"You won {tempUser.Wins} times and lost {tempUser.Losses} times, " +
                                 $"meaning you win {((tempUser.Wins + tempUser.Losses == 0) ? 0 : (tempUser.Wins * 100.0 / (tempUser.Wins + tempUser.Losses)))}% of fights and " +
                                 $"lose {((tempUser.Wins + tempUser.Losses == 0) ? 0 : (tempUser.Losses * 100.0 / (tempUser.Wins + tempUser.Losses)))}% of fights."
            });


            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.Success);
        }

        private void HandleScoreboardRequest()
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

            UserRepository userRepository = new UserRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

            User[] users = userRepository.ReadAllByElo();
            int rank = 1;

            List<UserScoreboard> stats = new List<UserScoreboard>();

            foreach(var u in users)
            {
                UserScoreboard temp = new UserScoreboard
                {
                    Rank = rank++,
                    Name = u.Username,
                    Elo = u.Elo
                };

                stats.Add(temp);
            }

            response = JsonConvert.SerializeObject(new
            {
                status = "success",
                scoreboard = stats
            });

            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.Success);
        }

        private void HandleTradingRequest()
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

            TradingRepository tradingRepository = new TradingRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");
            TradingDeal[] tradingDeals = null;

            try
            {
                tradingDeals = tradingRepository.AllTradingDeals();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Couldn't fetch trading deals",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            if (tradingDeals.Length == 0)
            {
                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "The request was fine, but there are no trading deals available",
                });

                ResponseHandler.SendResponse(writer, response, (int)ResponseCode.NoContent);

                return;
            }

            response = JsonConvert.SerializeObject(new
            {
                status = "success",
                trades = tradingDeals
            });

            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.Success);
        }
    }
}
