using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Objects;
using Newtonsoft.Json;
using ws2023_mtcg.Server.Repository;

namespace ws2023_mtcg.Server.Handlers
{
    internal class PostRequestHandler : IRequestHandler
    {
        string data;

        public PostRequestHandler(string req, string data)
        {
            if (req == null) 
                throw new ArgumentNullException();

            this.data = data;

            if (req.Contains("users"))
                HandleUserRequest();
        }

        public void HandleUserRequest()
        {
            User? tempUser = new User();
            tempUser = JsonConvert.DeserializeObject<User>(data);

            Console.WriteLine($"username {tempUser.Username}, password {tempUser.Password}");

            if (tempUser != null)
            {
                UserRepository userRepository = new UserRepository();
                userRepository.Create(tempUser);
            }
        }
    }
}
