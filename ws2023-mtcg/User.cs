using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg
{
    internal class User
    {
        private string _username { get; set; }
        private string _password { get; set; }
        private int _coins { get; set; } = 20;

        private Cards[] _stack { get; set; }
        private Cards[] _deck { get; set; }

        public User(string username/*, string password,*/)
        {
            _username = username;
            // _password = password;
        }
    }
}
