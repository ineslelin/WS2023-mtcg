using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg
{
    internal class Battle
    {
        private int _round;
        private User _playerOne;
        private User _playerTwo;

        public Battle(int round, User playerOne, User playerTwo)
        {
            _round = round;
            _playerOne = playerOne; 
            _playerTwo = playerTwo;
        }

        public void Fight()
        {
            int p1WinCount = 0;
            int p2WinCount = 0;

            while (_round < 100)
            {
                // also understand how that works, it works and lowkey it makes sense but UNDERSTAND YOUR CODE BRO
                if(!_playerOne.Deck.Any(card => card.IsAlive) || !_playerTwo.Deck.Any(card => card.IsAlive))
                    break;

                Random random = new Random();

                int randomP1Card, randomP2Card;

                do 
                {
                    randomP1Card = random.Next(0, _playerOne.Deck.Length);
                } 
                while (!_playerOne.Deck[randomP1Card].IsAlive);

                do
                {
                    randomP2Card = random.Next(0, _playerTwo.Deck.Length);
                }
                while (!_playerTwo.Deck[randomP2Card].IsAlive);

                // when youre in a writing ugly ass code competition and your opponent is me <3
                Console.WriteLine($"\n=====[ROUND {_round + 1}]=====\n" +
                    $" {_playerOne.Username}: {_playerOne.Deck[randomP1Card].Name} ({_playerOne.Deck[randomP1Card].Damage} damage) vs " +
                    $"{_playerTwo.Username}: {_playerTwo.Deck[randomP2Card].Name} ({_playerTwo.Deck[randomP2Card].Damage} damage): ");

                Cards winner = _playerOne.Deck[randomP1Card].Attack(_playerTwo.Deck[randomP2Card]);

                if (winner == _playerOne.Deck[randomP1Card])
                    p1WinCount++;
                else
                    p2WinCount++;

                _round++;

                //Console.WriteLine("\nPRESS ENTER TO CONTINUE!");
                //ConsoleKeyInfo key = Console.ReadKey();
                //if (key.Key == ConsoleKey.Enter)
                //    continue;
            }

            if (p1WinCount > p2WinCount)
                Console.WriteLine("Player 1 wins!");
            else if (p1WinCount < p2WinCount)
                Console.WriteLine("Player 2 wins!");
            else
                Console.WriteLine("It's a tie!");
        }
    }
}
