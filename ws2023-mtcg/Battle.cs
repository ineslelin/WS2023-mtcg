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

        // TO DO: make the little "x vs y: y wins" texts and iguess thats it fpr the battle logic???
        // i mean also ofc fix it so we have a winner, that would be nice
        // and understand the code lines you dont 100% understand
        // and if thats done, yk whats next???? server stuff, love myself some server stuff <3 /j
        public void Fight()
        {
            int p1WinCount = 0;
            int p2WinCount = 0;

            while (_round < 100)
            {
                // also understand how that works, it works and lowkey it makes sense but UNDERSTAND YOUR CODE BRO
                if(!_playerOne._deck.Any(card => card.IsAlive) || !_playerTwo._deck.Any(card => card.IsAlive))
                    break;

                Random random = new Random();

                int randomP1Card, randomP2Card;

                do 
                {
                    randomP1Card = random.Next(0, _playerOne._deck.Length);
                } 
                while (!_playerOne._deck[randomP1Card].IsAlive);

                do
                {
                    randomP2Card = random.Next(0, _playerTwo._deck.Length);
                }
                while (!_playerTwo._deck[randomP2Card].IsAlive);

                Cards winner = _playerOne._deck[randomP1Card].Attack(_playerTwo._deck[randomP2Card]);

                // UPDATE THIS PART DOESNT WORK, REDO IT
                (p1WinCount, p2WinCount) = (winner == _playerOne._deck[randomP1Card]) ? (p1WinCount++, p2WinCount) : (p1WinCount, p2WinCount++);

                _round++;
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
