using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void CardStats(Cards[] Deck)
        {
            Console.WriteLine($"{this.Username}'s Deck:");

            for (int i = 0; i < Deck.Length; i++)
            {
                if (!Deck[i].IsAlive)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{Deck[i].Name}: {Deck[i].Damage} Damage");

                    continue;
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{Deck[i].Name}: {Deck[i].Damage} Damage");
            }
            Console.ResetColor();

            Console.WriteLine("\nPRESS ANY KEY TO CONTINUE!");
            Console.ReadKey();
        }
    }
}
