using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Server.Repository;

namespace ws2023_mtcg.Server.Req
{
    internal class GetRequestHandler : IRequestHandler
    {
        StreamWriter writer;

        public GetRequestHandler(StreamWriter writer, string req) 
        { 
            if(req == null) 
                throw new ArgumentNullException(nameof(req));

            this.writer = writer;

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
