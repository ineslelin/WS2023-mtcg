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

namespace ws2023_mtcg.Server.Req
{
    internal class PostRequestHandler
    {
        string req;
        string data;

        TcpClient client;
        StreamReader reader;
        StreamWriter writer;

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
                ResponseHandler.SendErrorResponse(writer, "Invalid JSON.");
            }

            // Console.WriteLine($"username {tempUser.Username}, password {tempUser.Password}");

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
                    ResponseHandler.SendErrorResponse(writer, "Username already in use.");
                }

                ResponseHandler.SendResponse(writer, "User created.");
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
                ResponseHandler.SendErrorResponse(writer, "Invalid JSON.");
            }

            // Console.WriteLine($"username {tempUser.Username}, password {tempUser.Password}");

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
                    ResponseHandler.SendErrorResponse(writer, "Wrong username or password.");
                }

                ResponseHandler.SendResponse(writer, "User logged in.");
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
                ResponseHandler.SendErrorResponse(writer, "No token.");
            }

            try
            {
                string authHeader = TokenValidator.GetAuthHeader(req);

                if (!TokenValidator.CheckAdminToken(authHeader))
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Wrong token.");
            }

            List<Cards>? cards = new List<Cards>();

            try
            {
                cards = JsonConvert.DeserializeObject<List<Cards>>(data);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Invalid JSON");
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
                    ResponseHandler.SendErrorResponse(writer, "Card with that id already exists.");
                }
            }

            ResponseHandler.SendResponse(writer, "Package added.");
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
                ResponseHandler.SendErrorResponse(writer, "No token.");
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
                ResponseHandler.SendErrorResponse(writer, "No user with matching token.");
            }

            try
            {
                if(tempUser == null)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "User is null. (WHY???)");
            }

            try
            {
                if (tempUser.Coins < 5)
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Not enough money.");
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
                ResponseHandler.SendErrorResponse(writer, "No packages available");
            }

            int packageId = cardRepository.RetrieveSmallestId();
            Cards[]? package = null;

            try
            {
                package = cardRepository.RetrievePackage(packageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Couldn't retrieve package.");
            }

            if (package == null)
                throw new Exception();

            StackRepository stackRepository = new StackRepository();

            try
            {
                foreach (var p in package)
                {
                    p.Owner = tempUser.Username;
                    stackRepository.Create(p);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Coudln't add package to stack");
            }

            try
            {
                cardRepository.Delete(packageId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Coudln't add package to stack");
            }

            tempUser.Coins -= 5;

            try
            {
                userRepository.Update(tempUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                ResponseHandler.SendErrorResponse(writer, "Couldn't update user.");
            }

            ResponseHandler.SendResponse(writer, "Package acquired successfully.");
        }
    }
}
