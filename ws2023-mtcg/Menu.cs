using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.FightLogic.Enums;
using ws2023_mtcg.FightLogic;

namespace ws2023_mtcg
{
    internal class Menu
    {
        private string[] _menuOptions = { "Battle", "View Scoreboard", "Login", "Register", "Trade", "View Profile", "Exit" };
        private int _index = 0;

        public void Run()
        {
            ConsoleKeyInfo key;

            do
            {
                Console.Clear();
                DisplayMenu();

                key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        if (_index == 0)
                        {
                            _index = _menuOptions.Length - 1;
                            continue;
                        }
                        _index--;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if(_index == _menuOptions.Length - 1)
                        {
                            _index = 0;
                            continue;
                        }
                        _index++;
                        break;
                    default:
                        continue;
                }
            }
            while (key.Key != ConsoleKey.Enter);

            HandleCurrIndex();
        }

        private void DisplayMenu()
        {
            Console.WriteLine(" __  __                 _         _______            _ _              _____              _  _____                      \n" +
                              "|  \\/  |               | |       |__   __|          | (_)            / ____|            | |/ ____|                     \n" +
                              "| \\  / | ___  _ __  ___| |_ ___ _ __| |_ __ __ _  __| |_ _ __   __ _| |     __ _ _ __ __| | |  __  __ _ _ __ ___   ___ \n" +
                              "| |\\/| |/ _ \\| '_ \\/ __| __/ _ \\ '__| | '__/ _` |/ _` | | '_ \\ / _` | |    / _` | '__/ _` | | |_ |/ _` | '_ ` _ \\ / _ \\\n" +
                              "| |  | | (_) | | | \\__ \\ ||  __/ |  | | | | (_| | (_| | | | | | (_| | |___| (_| | | | (_| | |__| | (_| | | | | | |  __/\n" +
                              "|_|  |_|\\___/|_| |_|___/\\__\\___|_|  |_|_|  \\__,_|\\__,_|_|_| |_|\\__, |\\_____\\__,_|_|  \\__,_|\\_____|\\__,_|_| |_| |_|\\___|\n" +
                              "                                                                __/ |                                                  \n" +
                              "                                                               |___/                                                   \n\n");

            Console.WriteLine("Use the arrow keys or WASD to navigate the menu!\n");

            for (int i = 0; i < _menuOptions.Length; i++)
            {
                if (i == _index)
                    Console.WriteLine($" >> {_menuOptions[i]} ");
                else
                    Console.WriteLine($"    {_menuOptions[i]} ");
            }
        }

        public void HandleCurrIndex()
        {
            switch(_menuOptions[_index])
            {
                case "Battle": 
                    BattleOption();
                    break;

                case "View Scoreboard": 
                    ViewScoreboardOption();
                    break;

                case "Login":
                    LoginOption();
                    break;

                case "Register":
                    RegisterOption();
                    break;

                case "Trade":
                    TradeOption();
                    break;

                case "View Profile":
                    ViewProfileOption();
                    break;

                case "Exit": return;
            }

            Run();
        }

        public void BattleOption()
        {
            Console.Clear();

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
        }

        public void ViewScoreboardOption()
        {
            Console.Clear();
            Console.WriteLine("whoopsies, this hasnt been implemented yet");
        }

        public void LoginOption()
        {
            Console.Clear();
            Console.WriteLine("whoopsies, this hasnt been implemented yet");
        }

        public void RegisterOption()
        {
            Console.Clear();
            Console.WriteLine("whoopsies, this hasnt been implemented yet");
        }

        public void TradeOption()
        {
            Console.Clear();
            Console.WriteLine("whoopsies, this hasnt been implemented yet");
        }

        public void ViewProfileOption()
        {
            Console.Clear();
            Console.WriteLine("whoopsies, this hasnt been implemented yet");
        }
    }
}
