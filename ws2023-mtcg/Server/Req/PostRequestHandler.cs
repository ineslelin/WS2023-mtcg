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
        string data;
        StreamWriter writer;

        public PostRequestHandler(StreamWriter writer, string req, string data)
        {
            if (req == null) 
                throw new ArgumentNullException();

            this.data = data;
            this.writer = writer;

            if (req.Contains("users"))
                HandleUserRequest();

            if (req.Contains("sessions"))
                HandleSessionRequest();
        }

        public void HandleUserRequest()
        {
            User? tempUser = new User();
            tempUser = JsonConvert.DeserializeObject<User>(data);

            Console.WriteLine($"username {tempUser.Username}, password {tempUser.Password}");

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

        }
    }
}
