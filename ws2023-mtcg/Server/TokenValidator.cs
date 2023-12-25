using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Server.Res;

namespace ws2023_mtcg.Server
{
    internal class TokenValidator
    {
        public static void CheckTokenExistence(string req)
        {
            if (!req.Contains("Authorization"))
            {
                throw new Exception();
            }
        }

        public static bool CheckAdminToken(string authHeader)
        {
            if(SplitToken(authHeader) != "admin")
                return false;

            return true;
        }

        public static string GetAuthHeader(string req) 
        {
            string[] lines = req.Split("\n");
            string authHeader;

            foreach (string line in lines)
            {
                if (line.StartsWith("Authorization"))
                {
                    authHeader = line;
                    return authHeader;
                }
            }

            return null;
        }

        public static string SplitToken(string authHeader)
        {
            string[] parts = authHeader.Split(" ");

            if (parts.Length != 3 || parts[1] != "Bearer")
                throw new Exception();

            string[] token = parts[2].Split("-");

            if (token.Length != 2 || token[1] != "mtcgToken\r")
                throw new Exception();

            return token[0];
        }
    }
}
