using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Models
{
    internal class User
    {
        public int id;
        public string Username { get; set; }
        public string Password;
        public int Coins { get; set; }
        public int Elo { get; set; }

        public List<Cards> Stack = new List<Cards>();
        public List<Cards> Deck = new List<Cards>();

        public string Name { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

        public User()
        {
            Coins = 20;
            Elo = 100;
        }

        public void CardStats(List<Cards> Deck)
        {
            Console.WriteLine($"{Username}'s Deck:");

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

            List<Cards> tempStack = Stack;

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
            if (key.Key == ConsoleKey.Y)
            {
                return Deck;
            }

            Deck.Clear();
            return CreateDeck();
        }

        public void SetWinningELO()
        {
            Elo += 3;
        }

        public void SetLosingELO()
        {
            Elo -= 5;
        }

        public void AddToStack(List<Cards> wonCards)
        {
            foreach (Cards w in wonCards)
            {
                if (!w.IsAlive)
                {
                    w.IsAlive = true;
                    Stack.Add(w);
                }
            }
        }

        public void RemoveFromStack(List<Cards> lostCards)
        {
            foreach (Cards w in lostCards)
            {
                if (!w.IsAlive)
                {
                    Stack.Remove(w);
                }
            }
        }
    }
}
