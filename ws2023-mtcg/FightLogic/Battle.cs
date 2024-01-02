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
        //static private int _round;
        //static private User _playerOne;
        //static private User _playerTwo;

        //public Battle(int round, User playerOne, User playerTwo)
        //{
        //    _round = round;
        //    _playerOne = playerOne;
        //    _playerTwo = playerTwo;
        //}

        static string fightOutput;

        public static string Fight(int round, User playerOne, User playerTwo)
        {
            int p1WinCount = 0;
            int p2WinCount = 0;

            fightOutput = "";

            while (round < 100)
            {
                // also understand how that works, it works and lowkey it makes sense but UNDERSTAND YOUR CODE BRO
                if (!playerOne.Deck.Any(card => card.IsAlive) || !playerTwo.Deck.Any(card => card.IsAlive))
                    break;

                int randomP1Card = ChooseRandomCard(playerOne.Deck);
                int randomP2Card = ChooseRandomCard(playerTwo.Deck);

                // when youre in a writing ugly ass code competition and your opponent is me <3
                fightOutput += $"\n=====[ROUND {round}]=====\n" +
                               $" {playerOne.Username}: {playerOne.Deck[randomP1Card].Name} ({playerOne.Deck[randomP1Card].Damage} damage) vs " +
                               $"{playerTwo.Username}: {playerTwo.Deck[randomP2Card].Name} ({playerTwo.Deck[randomP2Card].Damage} damage): ";

                Cards winner = playerOne.Deck[randomP1Card].Attack(playerTwo.Deck[randomP2Card]);

                fightOutput += playerOne.Deck[randomP1Card].output;
                playerOne.Deck[randomP1Card].output = "";

                if (winner == playerOne.Deck[randomP1Card])
                    p1WinCount++;
                else
                    p2WinCount++;

                round++;
            }

            if (p1WinCount > p2WinCount)
            {
                fightOutput += $"\n\n{playerOne.Username} wins!";

                // unique mandatory feature: a random card receives a permanent buff and user gets money for winning
                Random random = new Random();
                int randomCard = random.Next(0, playerOne.Deck.Count);

                playerOne.Deck[randomCard].WinningDamageBuff();

                playerOne.Coins += 15;

                playerOne.SetWinningELO();
                playerTwo.SetLosingELO();
            }
            else if (p1WinCount < p2WinCount)
            {
                fightOutput += $"\n\n{playerTwo.Username} wins!";

                Random random = new Random();
                int randomCard = random.Next(0, playerTwo.Deck.Count);

                playerTwo.Deck[randomCard].WinningDamageBuff();

                playerTwo.Coins += 15;

                playerOne.SetLosingELO();
                playerTwo.SetWinningELO();
            }
            else
                fightOutput += "\n\nIt's a tie!";

            return fightOutput;
        }

        private static int ChooseRandomCard(List<Cards> deck)
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
