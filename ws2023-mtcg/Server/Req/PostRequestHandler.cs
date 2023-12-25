using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Models;
using Newtonsoft.Json;
using ws2023_mtcg.Server.Repository;
using ws2023_mtcg.Server.Res;

namespace ws2023_mtcg.Server.Req
{
    internal class PostRequestHandler : IRequestHandler
    {
        string req;
        string data;
        StreamWriter writer;

        public PostRequestHandler(StreamWriter writer, string req, string data)
        {
            if (req == null) 
                throw new ArgumentNullException();

            this.data = data;
            this.writer = writer;
            this.req = req;

            if (req.Contains("users"))
                HandleUserRequest();

            if (req.Contains("sessions"))
                HandleSessionRequest();

            if (req.Contains("packages") && !req.Contains("transactions"))
                HandlePackageRequest();
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
                try
                {
                    foreach(var c in cards)
                    {
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
    }
}
