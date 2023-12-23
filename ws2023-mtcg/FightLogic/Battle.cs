using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Models;

namespace ws2023_mtcg.FightLogic
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

        // TODO: do the whole giving cards to winner thing, also maybe finally implement elo
        public void Fight()
        {
            _playerOne.CreateDeck();
            _playerTwo.CreateDeck();

            int p1WinCount = 0;
            int p2WinCount = 0;

            while (_round < 100)
            {
                Console.Clear();

                // also understand how that works, it works and lowkey it makes sense but UNDERSTAND YOUR CODE BRO
                if (!_playerOne.Deck.Any(card => card.IsAlive) || !_playerTwo.Deck.Any(card => card.IsAlive))
                    break;

                int randomP1Card = ChooseRandomCard(_playerOne.Deck);
                int randomP2Card = ChooseRandomCard(_playerTwo.Deck);

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

                Console.WriteLine("\nPRESS ENTER TO CONTINUE!\nPRESS S TO LOOK AT THE CARD STATS!");
                ConsoleKeyInfo key = Console.ReadKey();

                if(key.Key == ConsoleKey.S)
                {
                    _playerOne.CardStats(_playerOne.Deck);
                    _playerTwo.CardStats(_playerTwo.Deck);
                }
                if (key.Key == ConsoleKey.Enter)
                    continue;
            }

            if (p1WinCount > p2WinCount)
            {
                Console.WriteLine("Player 1 wins!");
                _playerOne.SetWinningELO();
                _playerTwo.SetLosingELO();

                _playerOne.AddToStack(_playerTwo.Deck);
                _playerTwo.RemoveFromStack(_playerTwo.Deck);
            }
            else if (p1WinCount < p2WinCount)
            {
                Console.WriteLine("Player 2 wins!");
                _playerOne.SetLosingELO();
                _playerTwo.SetWinningELO();

                _playerOne.RemoveFromStack(_playerOne.Deck);
                _playerTwo.AddToStack(_playerOne.Deck);
            }
            else
                Console.WriteLine("It's a tie!");

            _playerOne.Deck.Clear();
            _playerTwo.Deck.Clear();
        }

        private int ChooseRandomCard(List<Cards> deck)
        {
            Random random = new Random();
            int randomCard;

            do
            {
                randomCard = random.Next(0, deck.Count);
            }
            while (!deck[randomCard].IsAlive);

            return randomCard;
        }
    }
}
