using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Server.Repository;

namespace ws2023_mtcg.Server.RequestHandler
{
    internal class GetRequestHandler : IRequestHandler
    {
        public GetRequestHandler(string req) 
        { 
            if(req == null) 
                throw new ArgumentNullException(nameof(req));

            if(req.Contains("users"))
            {
                HandleUserRequest();
            }
        }

        public void HandleUserRequest()
        {
            UserRepository userRepository = new UserRepository();
            // userRepository.Read();
        }
    }
}
