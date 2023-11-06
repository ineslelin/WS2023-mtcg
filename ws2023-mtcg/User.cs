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

        public List<Cards> Stack = new List<Cards>();
        public List<Cards> Deck = new List<Cards>();

        public User(string username/*, string password, Cards[] _stack*/, List<Cards> stack)
        {
            Username = username;
            // _password = password;
            Stack = stack;
        }

        public void CardStats(List<Cards> Deck)
        {
            Console.WriteLine($"{this.Username}'s Deck:");

            for (int i = 0; i < Deck.Count; i++)
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

        public List<Cards> CreateDeck()
        {
            // todo: make errors impossible by returning error messages for wrong input, namely at places hightlighted with !!!

            List<Cards> tempStack = this.Stack;

            while (Deck.Count < 4)
            {
                Console.WriteLine($"{Username}'s stack:");
                // for easiness' sake im gonna make the stacks using the good old number input
                for (int i = 0; i < tempStack.Count; i++)
                {
                    Console.WriteLine($"[{i}] {tempStack[i].Name}: {tempStack[i].Damage} Damage, {tempStack[i].Element}");
                }

                // !!!
                int chosenCard = Convert.ToInt32(Console.ReadLine());

                Deck.Add(tempStack[chosenCard]);
                tempStack.RemoveAt(chosenCard);

                Console.Clear();
            }

            // !!!
            Console.WriteLine("Are you satisfied with this deck? (Y/N)");

            for (int i = 0; i < Deck.Count; i++)
            {
                Console.WriteLine($"{Deck[i].Name}: {Deck[i].Damage} Damage, {Deck[i].Element}");
            }

            ConsoleKeyInfo key = Console.ReadKey();
            if(key.Key == ConsoleKey.Y)
            {
                return this.Deck;
            }
                
            Deck.Clear();
            return CreateDeck();
        }
    }
}
