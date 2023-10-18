// IDEA FOR UNIQUE FEATURE
// team modus where two or maybe even more users can play against bots or other users
// cards are randomly chosen, they basically morph together and damage gets added up, element type changes to the
// "stronger" element
//      ex. fire and water cards vs. normal and water => water (water stronger than fire) v. normal (normal stronger than water)
// ALTERNATIVELY each card chooses another random card from the opponent and they fight each other at the same time and its possible that
// either both of attacking team or both of opposing team are knocked out immediately or its a last one standing situation and 
// the two last one fight each other idk idk

using System;
using ws2023_mtcg.MonsterCards;
using ws2023_mtcg.MonsterCards.ElfTypes;
using ws2023_mtcg.MonsterCards.GoblinTypes;
using ws2023_mtcg.MonsterCards.TrollTypes;
using ws2023_mtcg.SpellCards;

namespace ws2023_mtcg
{
    class Program
    {
        static void Main(string[] args)
        {
            Cards[] playerOneCards = { new Dragon(), new WaterElf(), new FireElf(), new RegularElf(), new FireGoblin(), new RegularGoblin(), 
                                       new WaterGoblin(), new Knight(), new Kraken(), new Ork(), new FireTroll(), new RegularTroll(), 
                                       new WaterTroll(), new Wizard(), new FireSpell(), new RegularSpell(), new WaterSpell() };
            Cards[] playerTwoCards = { new Dragon(), new WaterElf(), new FireElf(), new RegularElf(), new FireGoblin(), new RegularGoblin(),
                                       new WaterGoblin(), new Knight(), new Kraken(), new Ork(), new FireTroll(), new RegularTroll(),
                                       new WaterTroll(), new Wizard(), new FireSpell(), new RegularSpell(), new WaterSpell() };

            User PlayerOne = new User("Player 1", playerOneCards);
            User PlayerTwo = new User("Player 2", playerTwoCards);

            // ooooooh this isnt doen well yet but trust. trust. truuuuust.
            Battle battle = new Battle(0, PlayerOne, PlayerTwo);

            battle.Fight();
        }
    }
}
