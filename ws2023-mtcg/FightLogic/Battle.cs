using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Models;
using static System.Net.Mime.MediaTypeNames;

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

        string fightOutput;
        bool eloUpdated = false;

        public string Fight(int round, User playerOne, User playerTwo)
        {
            int p1WinCount = 0;
            int p2WinCount = 0;
            eloUpdated = false;

            fightOutput = "";

            while (round <= 100)
            {
                if (!playerOne.Deck.Any(card => card.IsAlive) || !playerTwo.Deck.Any(card => card.IsAlive))
                    break;

                int randomP1Card = ChooseRandomCard(playerOne.Deck);
                int randomP2Card = ChooseRandomCard(playerTwo.Deck);

                bool P1Mega = false;
                bool P2Mega = false;

                if (MegaCard())
                {
                    P1Mega = true;
                    playerOne.Deck[randomP1Card].Damage = playerOne.Deck[randomP1Card].MegaBuff();
                    fightOutput += $"\n{playerOne.Deck[randomP1Card].Name} received a Mega Buff!\n";
                }

                if (MegaCard())
                {
                    P2Mega = true;
                    playerTwo.Deck[randomP2Card].Damage = playerTwo.Deck[randomP2Card].MegaBuff();
                    fightOutput += $"\n{playerTwo.Deck[randomP2Card].Name} received a Mega Buff!\n";
                }

                // when youre in a writing ugly ass code competition and your opponent is me <3
                fightOutput += $"\n=====[ROUND {round}]=====\n" +
                               $" {playerOne.Username}: {playerOne.Deck[randomP1Card].Name} ({playerOne.Deck[randomP1Card].Damage} damage) vs " +
                               $"{playerTwo.Username}: {playerTwo.Deck[randomP2Card].Name} ({playerTwo.Deck[randomP2Card].Damage} damage): ";

                Cards winner = playerOne.Deck[randomP1Card].Attack(playerTwo.Deck[randomP2Card]);

                fightOutput += playerOne.Deck[randomP1Card].output;
                playerOne.Deck[randomP1Card].output = "";

                Console.WriteLine($"{playerOne.Deck[randomP1Card].IsAlive} {playerTwo.Deck[randomP2Card].IsAlive}");

                if(P1Mega)
                {
                    P1Mega = false;
                    playerOne.Deck[randomP1Card].Damage = playerOne.Deck[randomP1Card].ResetMegaBuff();
                }

                if (P2Mega)
                {
                    P2Mega = false;
                    playerTwo.Deck[randomP2Card].Damage = playerTwo.Deck[randomP2Card].ResetMegaBuff();
                }

                if (winner == playerOne.Deck[randomP1Card])
                {
                    //playerTwo.Deck[randomP2Card].IsAlive = true;
                    //playerOne.Deck.Add(playerTwo.Deck[randomP2Card]);
                    //playerTwo.Deck.Remove(playerTwo.Deck[randomP2Card]);

                    p1WinCount++;
                }
                else if(winner == playerTwo.Deck[randomP2Card])
                {
                    //playerOne.Deck[randomP1Card].IsAlive = true;
                    //playerTwo.Deck.Add(playerOne.Deck[randomP1Card]);
                    //playerOne.Deck.Remove(playerOne.Deck[randomP1Card]);

                    p2WinCount++;
                }

                round++;
            }

            fightOutput += "\n=====[BATTLE END]=====\n";

            if (p1WinCount > p2WinCount)
            {
                fightOutput += $"{playerOne.Username} wins!\n";

                if(!eloUpdated)
                {
                    playerOne.Coins += 15;

                    playerOne.Elo += 3;
                    playerOne.Wins += 1;

                    playerTwo.Elo -= 5;
                    playerTwo.Losses += 1;

                    playerOne.Deck = Cards.ResetCurse(playerOne.Deck);
                    playerTwo.Deck = Cards.ResetCurse(playerTwo.Deck);

                    eloUpdated = true;
                }

                return fightOutput;
            }
            else if (p1WinCount < p2WinCount)
            {
                fightOutput += $"{playerTwo.Username} wins!\n";

                if(!eloUpdated)
                {
                    playerTwo.Coins += 15;

                    playerTwo.Elo += 3;
                    playerTwo.Wins += 1;

                    playerOne.Elo -= 5;
                    playerOne.Losses += 1;

                    playerOne.Deck = Cards.ResetCurse(playerOne.Deck);
                    playerTwo.Deck = Cards.ResetCurse(playerTwo.Deck);

                    eloUpdated = true;
                }
                
                return fightOutput;
            }
            else
            {
                fightOutput += "It's a tie!\n";
                return fightOutput;
            }
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

        private static bool MegaCard()
        {
            Random random = new Random();
            int randomValue = random.Next(0, 1000);

            if(randomValue >= 990)
                return true;

            return false;
        }
    }
}
