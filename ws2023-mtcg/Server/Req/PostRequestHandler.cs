using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Models;
using Newtonsoft.Json;
using ws2023_mtcg.Server.Repository;
using ws2023_mtcg.Server.Res;
using System.Net.Sockets;
using ws2023_mtcg.FightLogic;
using System.Text.RegularExpressions;

namespace ws2023_mtcg.Server.Req
{
    internal class PostRequestHandler
    {
        string req;
        string data;
        string response;

        TcpClient client;
        StreamReader reader;
        StreamWriter writer;

        static List<User> lobby = new List<User> ();
        static string output = "";
        bool battleHandled = false;
        ManualResetEvent waitForPlayers = new ManualResetEvent(false);

        public PostRequestHandler(TcpClient client, StreamReader reader, StreamWriter writer, string req)
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

            if (route[1] == "/users")
            {
                data = serverCommands.RetrieveData();
                HandleUserRequest();
            }

            if (route[1] == "/sessions")
            {
                data = serverCommands.RetrieveData();
                HandleSessionRequest();
            }

            if (route[1] == "/packages")
            {
                data = serverCommands.RetrieveData();
                HandlePackageRequest();
            }

            if (route[1] == "/transactions/packages")
            {
                HandleTransactionRequest();
            }

            if (route[1] == "/battles")
            {
                HandleBattleRequest();
            }

            if (route[1] == "/tradings")
            {
                data = serverCommands.RetrieveData();
                HandleTradingRequest();
            }

            if (Regex.IsMatch(route[1], @"/tradings/[a-zA-Z0-9-]*"))
            {
                string[] id = route[1].Split("/");
                data = serverCommands.RetrieveData();
                HandleTransactionRequest(id[2]);
            }
        }

        private void HandleUserRequest()
        {
            User? tempUser = new User();

            try
            {
                tempUser = JsonConvert.DeserializeObject<User>(data);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Invalid JSON"
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            if (tempUser != null)
            {
                UserRepository userRepository = new UserRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

                tempUser.Password = PasswordSecurity.EncryptPassword(tempUser.Password);

                try
                {
                    userRepository.Create(tempUser);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");

                    response = JsonConvert.SerializeObject(new
                    {
                        status = "error",
                        message = "User with same username already registered"
                    });

                    ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.AlreadyExists);

                    return;
                }

            }

            response = JsonConvert.SerializeObject( new
            {
                status = "success",
                message = "User successfully created",
                user = new UserCredentials 
                {
                    Username = tempUser.Username,
                    Password = tempUser.Password
                }
            });
            
            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.CreationSuccess);
        }

        private void HandleSessionRequest()
        {
            User? tempUser = new User();

            try
            {
                tempUser = JsonConvert.DeserializeObject<User>(data);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Invalid JSON"
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            if (tempUser != null)
            {
                UserRepository userRepository = new UserRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

                tempUser.Password = PasswordSecurity.EncryptPassword(tempUser.Password);

                try
                {
                    User userMatch = userRepository.Read(tempUser.Username);

                    if(userMatch.Password != tempUser.Password)
                        throw new Exception();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");

                    response = JsonConvert.SerializeObject(new
                    {
                        status = "error",
                        message = "Invalid username/password provided"
                    });

                    ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Unauthorized);

                    return;
                }

                response = JsonConvert.SerializeObject(new
                {
                    status = "success",
                    message = "User login successful",
                    user = new UserCredentials
                    {
                        Username = tempUser.Username,
                        Password = tempUser.Password
                    },
                    token = $"{tempUser.Username}-mtcgToken"
                });

                ResponseHandler.SendResponse(writer, response, (int)ResponseCode.Success);
            }
        }

        private void HandlePackageRequest()
        {
            string authHeader = "";

            try
            {
                TokenValidator.CheckTokenExistence(req);
                authHeader = TokenValidator.GetAuthHeader(req);
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

            try
            {
                if (!TokenValidator.CheckAdminToken(authHeader))
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Provided user is not \"admin\"",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Forbidden);

                return;
            }

            List<Cards>? cards = new List<Cards>();

            try
            {
                cards = JsonConvert.DeserializeObject<List<Cards>>(data);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Invalid JSON"
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            CardRepository cardRepository = new CardRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

            if(cards != null && cards.Count > 0)
            {
                int packageId = cardRepository.RetrieveHighestId();
                packageId++;

                try
                {
                    foreach(var c in cards)
                    {
                        c.Package = packageId;
                        cardRepository.Create(c);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");

                    response = JsonConvert.SerializeObject(new
                    {
                        status = "error",
                        message = "At least one card in the packages already exists"
                    });

                    ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.AlreadyExists);

                    return;
                }
            }

            List<Card> package = new List<Card>();

            foreach (var c in cards)
            {
                Card card = new Card
                {
                    Id = c.Id,
                    Name = c.Name,
                    Damage = c.Damage
                };

                package.Add(card);
            }

            response = JsonConvert.SerializeObject(new
            {
                status = "success",
                message = "Package and cards successfully created",
                package = package
            });

            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.CreationSuccess);
        }

        private void HandleTransactionRequest()
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

            User? tempUser = new User();
            UserRepository userRepository = new UserRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

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
                    message = "No user with matching credentials found",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.NotFound);

                return;
            }

            try
            {
                if (tempUser.Coins < 5)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}"); 
                
                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Not enough money for buying a card package",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Forbidden);

                return;
            }

            CardRepository cardRepository = new CardRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

            try
            {
                if (!cardRepository.CheckForPackages())
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "No card package available for buying",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.NotFound);

                return;
            }

            int packageId = cardRepository.RetrieveSmallestId();
            Cards[]? package = null;
            StackRepository stackRepository = new StackRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

            try
            {
                package = cardRepository.ReadByPackage(packageId);

                if (package == null)
                    throw new Exception();

                foreach (var p in package)
                {
                    p.Owner = tempUser.Username;
                    stackRepository.Create(p);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Couldn't add package to user",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            cardRepository.Delete(packageId);

            try
            {
                tempUser.Coins -= 5;
                userRepository.Update(tempUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}"); 
                
                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Couldn't update user",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            List<Card> acquiredPackage = new List<Card>();

            foreach (var p in package)
            {
                Card card = new Card
                {
                    Id = p.Id,
                    Name = p.Name,
                    Damage = p.Damage
                };

                acquiredPackage.Add(card);
            }

            response = JsonConvert.SerializeObject(new
            {
                status = "success",
                message = "A package has been successfully bought",
                package = acquiredPackage
            });

            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.Success);
        }

        private void HandleBattleRequest()
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

            User? tempUser = new User();
            UserRepository userRepository = new UserRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");
            DeckRepository deckRepository = new DeckRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

            try
            {
                tempUser = userRepository.Read(username);
                tempUser.Deck = deckRepository.Read(username).ToList();

                lock(lobby)
                {
                    lobby.Add(tempUser);

                    if (lobby.Count == 2)
                    {
                        waitForPlayers.Set();
                    }
                }


                waitForPlayers.Reset();
                battleHandled = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "No user with matching credentials found",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.NotFound);

                return;
            }

            while (lobby.Count != 2);

            User player1 = lobby[0];
            User player2 = lobby[1];

            try
            {
                Monitor.Enter(this);

                if(!battleHandled)
                {
                    lock(this)
                    {
                        Battle battle = new Battle();
                        output = battle.Fight(1, player1, player2);
                        battleHandled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Battle failed",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }
            finally
            {
                Monitor.Exit(this);
                battleHandled = false;
            }

            try
            {
                userRepository.Update(player1);
                userRepository.Update(player2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Couldn't update user stats",
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            ResponseHandler.SendPlaintextResponse(writer, output, (int)ResponseCode.Success);

            lobby.Clear();
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

            TradingDeal deal = new TradingDeal();

            try
            {
                deal = JsonConvert.DeserializeObject<TradingDeal>(data);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "Invalid JSON"
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            TradingRepository tradingRepository = new TradingRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");
            StackRepository stackRepository = new StackRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");
            DeckRepository deckRepository = new DeckRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

            TradingDeal temp = new TradingDeal();

            try
            {
                temp = tradingRepository.Read(deal.Id);

                if (temp.Id != null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "A deal with this deal ID already exists."
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.AlreadyExists);

                return;
            }

            try
            {
                if (deckRepository.ReadById(deal.CardToTrade) != null || stackRepository.ReadById(deal.CardToTrade) == null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "The deal contains a card that is not owned by the user or locked in the deck."
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Forbidden);

                return;
            }

            tradingRepository.Create(deal, username);

            response = JsonConvert.SerializeObject(new
            {
                status = "success",
                deal = deal
            });

            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.CreationSuccess);
        }

        private void HandleTransactionRequest(string id)
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

            // this is the card the user offers
            Cards offer = new Cards();
            offer.Id = data;
            // this is the card that is up for selling
            TradingDeal deal = new TradingDeal();

            TradingRepository tradingRepository = new TradingRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

            try
            {
                deal = tradingRepository.Read(id);

                if (deal.Id == null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "The provided deal ID was not found."
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.NotFound);

                return;
            }

            try
            {
                if (deal.Username == username)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "You can't trade with yourself"
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Error);

                return;
            }

            StackRepository stackRepository = new StackRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");
            DeckRepository deckRepository = new DeckRepository("Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true");

            try
            {
                if(deckRepository.ReadById(offer.Id) != null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "The offered card is locked in the deck"
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Forbidden);

                return;
            }

            try
            {
                offer = stackRepository.ReadById(offer.Id);

                if (offer == null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "The offered card doesn't belong to you"
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Forbidden);

                return;
            }

            try
            {
                if (offer.Type != deal.Type || offer.Damage < deal.MinimumDamage)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");

                response = JsonConvert.SerializeObject(new
                {
                    status = "error",
                    message = "The offered card doesn't meet the requirements"
                });

                ResponseHandler.SendErrorResponse(writer, response, (int)ResponseCode.Forbidden);

                return;
            }

            Cards cardDeal = new Cards()
            {
                Id = deal.CardToTrade
            };

            cardDeal = stackRepository.ReadById(cardDeal.Id);
            cardDeal.Owner = username;

            offer.Owner = deal.Username;

            stackRepository.Update(cardDeal);
            stackRepository.Update(offer);

            tradingRepository.Delete(deal.Id);

            response = JsonConvert.SerializeObject(new
            {
                status = "success",
                message = "Trading deal successfully executed."
            });

            ResponseHandler.SendResponse(writer, response, (int)ResponseCode.Success);
        }
    }
}
