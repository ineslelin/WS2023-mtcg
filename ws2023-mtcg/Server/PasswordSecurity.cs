using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Server
{
    internal class PasswordSecurity
    {
        public static string EncryptPassword(string password)
        {
            using SHA256 sHA256 = SHA256.Create();

            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sHA256.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}
