using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Server.Res
{
    enum ResponseCode
    {
        Success = 200,
        CreationSuccess = 201,
        Error = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        AlreadyExists = 409,
    }
}
