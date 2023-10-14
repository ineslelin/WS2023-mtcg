using System;
using ws2023_mtcg.MonsterCards;
using ws2023_mtcg.SpellCards;

namespace ws2023_mtcg
{
    class Program
    {
        static void Main(string[] args)
        {
            Cards[] playerOneCards = { new Dragon(), new Elf(), new RegularSpell() };
            Cards[] playerTwoCards = { new Goblin(), new Kraken(), new WaterSpell() };

            User PlayerOne = new User("Player 1", playerOneCards);
            User PlayerTwo = new User("Player 2", playerTwoCards);

            // ooooooh this isnt doen well yet but trust. trust. truuuuust.
            Battle battle = new Battle(0, PlayerOne, PlayerTwo);

            battle.Fight();
        }
    }
}
