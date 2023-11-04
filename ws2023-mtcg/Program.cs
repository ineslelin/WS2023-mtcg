// IDEA FOR UNIQUE FEATURE
// team modus where two or maybe even more users can play against bots or other users
// cards are randomly chosen, they basically morph together and damage gets added up, element type changes to the
// "stronger" element
//      ex. fire and water cards vs. normal and water => water (water stronger than fire) v. normal (normal stronger than water)
// ALTERNATIVELY each card chooses another random card from the opponent and they fight each other at the same time and its possible that
// either both of attacking team or both of opposing team are knocked out immediately or its a last one standing situation and 
// the two last one fight each other idk idk

using System;
using ws2023_mtcg.FightLogic;
using ws2023_mtcg.FightLogic.Enums;
using ws2023_mtcg.Server;

namespace ws2023_mtcg
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer server = new HttpServer(8080);
            server.Start();

            Cards[] playerOneCards = { new Cards(MonsterType.Dragon),
                                       new Cards(MonsterType.FireElf),
                                       new Cards(MonsterType.RegularElf),
                                       new Cards(MonsterType.WaterElf),
                                       new Cards(MonsterType.FireGoblin),
                                       new Cards(MonsterType.RegularGoblin),
                                       new Cards(MonsterType.WaterGoblin),
                                       new Cards(MonsterType.FireTroll),
                                       new Cards(MonsterType.RegularTroll),
                                       new Cards(MonsterType.WaterTroll),
                                       new Cards(MonsterType.Knight),
                                       new Cards(MonsterType.Kraken),
                                       new Cards(MonsterType.Ork),
                                       new Cards(MonsterType.Wizard),
                                       new Cards(SpellType.FireSpell),
                                       new Cards(SpellType.RegularSpell),
                                       new Cards(SpellType.WaterSpell), };
            Cards[] playerTwoCards = { new Cards(MonsterType.Dragon),
                                       new Cards(MonsterType.FireElf),
                                       new Cards(MonsterType.RegularElf),
                                       new Cards(MonsterType.WaterElf),
                                       new Cards(MonsterType.FireGoblin),
                                       new Cards(MonsterType.RegularGoblin),
                                       new Cards(MonsterType.WaterGoblin),
                                       new Cards(MonsterType.FireTroll),
                                       new Cards(MonsterType.RegularTroll),
                                       new Cards(MonsterType.WaterTroll),
                                       new Cards(MonsterType.Knight),
                                       new Cards(MonsterType.Kraken),
                                       new Cards(MonsterType.Ork),
                                       new Cards(MonsterType.Wizard),
                                       new Cards(SpellType.FireSpell),
                                       new Cards(SpellType.RegularSpell),
                                       new Cards(SpellType.WaterSpell), };

            User PlayerOne = new User("Player 1", playerOneCards);
            User PlayerTwo = new User("Player 2", playerTwoCards);

            // ooooooh this isnt doen well yet but trust. trust. truuuuust.
            Battle battle = new Battle(0, PlayerOne, PlayerTwo);

            battle.Fight();

            server.Stop();
        }
    }
}
