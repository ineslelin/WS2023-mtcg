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

namespace ws2023_mtcg.Server.Req
{
    internal class PostRequestHandler
    {
        string req;
        string data;

        TcpClient client;
        StreamReader reader;
        StreamWriter writer;

        static List<User> lobby = new List<User> ();
        static string output = "";
        static Battle battle;

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
        }

        public void HandleUserRequest()
        {
            User? tempUser = new User();

            try
            {
                tempUser = JsonConvert.DeserializeObject<User>(data);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Invalid JSON.", 400);

                return;
            }

            if (tempUser != null)
            {
                UserRepository userRepository = new UserRepository();

                tempUser.Password = PasswordSecurity.EncryptPassword(tempUser.Password);

                try
                {
                    userRepository.Create(tempUser);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                    ResponseHandler.SendErrorResponse(writer, "Username already in use.", 409);

                    return;
                }

                ResponseHandler.SendResponse(writer, "User created.", 201);
            }
        }

        public void HandleSessionRequest()
        {
            User? tempUser = new User();

            try
            {
                tempUser = JsonConvert.DeserializeObject<User>(data);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Invalid JSON.", 400);

                return;
            }

            if (tempUser != null)
            {
                UserRepository userRepository = new UserRepository();

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
                    ResponseHandler.SendErrorResponse(writer, "Wrong username or password.", 401);

                    return;
                }

                ResponseHandler.SendResponse(writer, $"User logged in. Session-Token: {tempUser.Username}-mtcgToken", 200);
            }
        }

        public void HandlePackageRequest()
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

            try
            {
                if (!TokenValidator.CheckAdminToken(authHeader))
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "User is not admin.", 403);

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
                ResponseHandler.SendErrorResponse(writer, "Invalid JSON", 400);

                return;
            }

            CardRepository cardRepository = new CardRepository();

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
                    ResponseHandler.SendErrorResponse(writer, "Card with that id already exists.", 409);

                    return;
                }
            }

            ResponseHandler.SendResponse(writer, "Package added.", 201);
        }

        public void HandleTransactionRequest()
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

            User? tempUser = new User();

            UserRepository userRepository = new UserRepository();

            try
            {
                tempUser = userRepository.Read(username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "No user with matching token.", 401);

                return;
            }

            try
            {
                if(tempUser == null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "User is null. (WHY???)", 400);

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
                ResponseHandler.SendErrorResponse(writer, "Not enough money.", 403);

                return;
            }

            CardRepository cardRepository = new CardRepository();

            try
            {
                if (!cardRepository.CheckForPackages())
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "No packages available", 404);

                return;
            }

            int packageId = cardRepository.RetrieveSmallestId();
            Cards[]? package = null;

            try
            {
                package = cardRepository.ReadByPackage(packageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Couldn't retrieve package.", 400);

                return;
            }


            StackRepository stackRepository = new StackRepository();

            try
            {
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
                ResponseHandler.SendErrorResponse(writer, "Coudln't add package to stack", 400);

                return;
            }

            try
            {
                cardRepository.Delete(packageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Coudln't delete package from stack", 400);

                return;
            }

            tempUser.Coins -= 5;

            try
            {
                userRepository.Update(tempUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Couldn't update user.", 400);

                return;
            }

            ResponseHandler.SendResponse(writer, "Package acquired successfully.", 200);
        }

        public void HandleBattleRequest()
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

            Monitor.Enter(this);
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
            Monitor.Exit(this);

            string username = "";

            Monitor.Enter(this);
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

            User? tempUser = new User();
            UserRepository userRepository = new UserRepository();
            DeckRepository deckRepository = new DeckRepository();

            try
            {
                tempUser = userRepository.Read(username);
                tempUser.Deck = deckRepository.Read(username).ToList();
                lobby.Add(tempUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "No user with matching token.", 401);

                return;
            }
            Monitor.Exit(this);

            while(lobby.Count < 2)
            {
                Console.WriteLine("Waiting for second user to join...");
                Thread.Sleep(1000);
            }

            User player1 = lobby[0];
            User player2 = lobby[1];
            lobby.RemoveAt(0);
            lobby.RemoveAt(0);

            try
            {
                output = Battle.Fight(1, player1, player2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "No user with matching token.", 401);

                return;
            }

            try
            {
                userRepository.Update(player1);
                userRepository.Update(player2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Couldnt' update player stats", 400);

                return;
            }

            ResponseHandler.SendResponse(writer, output, 200);
        }
    }
}
