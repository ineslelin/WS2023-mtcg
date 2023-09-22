using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg
{
    internal class User
    {
        private string Username { get; set; }
        private string Password { get; set; }
        private int Coins { get; set; } = 20;

        ICards[] Stack { get; set; }
        ICards[] Deck { get; set; }
    }
}
