using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.FightLogic;

namespace ws2023_mtcg
{
    internal class User
    {
        public string Username { get; private set; }
        private string _password;
        public int Coins { get; set; } = 20;

        public Cards[] Stack { get; set; }
        public Cards[] Deck { get; set; }

        public User(string username/*, string password, Cards[] _stack*/, Cards[] deck)
        {
            Username = username;
            // _password = password;
            Deck = deck;
        }
    }
}
