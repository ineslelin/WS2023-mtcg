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
                              "                                                               |___/                                                   \n");

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

            List<Cards> playerOneCards = new List<Cards>();
            
            playerOneCards.Add(new Cards(MonsterType.Dragon));
            playerOneCards.Add(new Cards(MonsterType.FireElf));
            playerOneCards.Add(new Cards(MonsterType.RegularElf));
            playerOneCards.Add(new Cards(MonsterType.WaterElf));
            playerOneCards.Add(new Cards(MonsterType.FireGoblin));
            playerOneCards.Add(new Cards(MonsterType.RegularGoblin));
            playerOneCards.Add(new Cards(MonsterType.WaterGoblin));
            playerOneCards.Add(new Cards(MonsterType.FireTroll));
            playerOneCards.Add(new Cards(MonsterType.RegularTroll));
            playerOneCards.Add(new Cards(MonsterType.WaterTroll));
            playerOneCards.Add(new Cards(MonsterType.Knight));
            playerOneCards.Add(new Cards(MonsterType.Kraken));
            playerOneCards.Add(new Cards(MonsterType.Ork));
            playerOneCards.Add(new Cards(MonsterType.Wizard));
            playerOneCards.Add(new Cards(SpellType.FireSpell));
            playerOneCards.Add(new Cards(SpellType.RegularSpell));
            playerOneCards.Add(new Cards(SpellType.WaterSpell));

            List<Cards> playerTwoCards = new List<Cards>();

            playerTwoCards.Add(new Cards(MonsterType.Dragon));
            playerTwoCards.Add(new Cards(MonsterType.FireElf));
            playerTwoCards.Add(new Cards(MonsterType.RegularElf));
            playerTwoCards.Add(new Cards(MonsterType.WaterElf));
            playerTwoCards.Add(new Cards(MonsterType.FireGoblin));
            playerTwoCards.Add(new Cards(MonsterType.RegularGoblin));
            playerTwoCards.Add(new Cards(MonsterType.WaterGoblin));
            playerTwoCards.Add(new Cards(MonsterType.FireTroll));
            playerTwoCards.Add(new Cards(MonsterType.RegularTroll));
            playerTwoCards.Add(new Cards(MonsterType.WaterTroll));
            playerTwoCards.Add(new Cards(MonsterType.Knight));
            playerTwoCards.Add(new Cards(MonsterType.Kraken));
            playerTwoCards.Add(new Cards(MonsterType.Ork));
            playerTwoCards.Add(new Cards(MonsterType.Wizard));
            playerTwoCards.Add(new Cards(SpellType.FireSpell));
            playerTwoCards.Add(new Cards(SpellType.RegularSpell));
            playerTwoCards.Add(new Cards(SpellType.WaterSpell));

            User PlayerOne = new User("Player 1", playerOneCards);
            User PlayerTwo = new User("Player 2", playerTwoCards);

            // ooooooh this isnt doen well yet but trust. trust. truuuuust.
            Battle battle = new Battle(0, PlayerOne, PlayerTwo);

            battle.Fight();

            Console.ReadKey();
        }

        public void ViewScoreboardOption()
        {
            Console.Clear();
            Console.WriteLine("whoopsies, this hasnt been implemented yet");

            Console.ReadKey();
        }

        public void LoginOption()
        {
            Console.Clear();
            Console.WriteLine("whoopsies, this hasnt been implemented yet");

            Console.ReadKey();
        }

        public void RegisterOption()
        {
            Console.Clear();
            Console.WriteLine("whoopsies, this hasnt been implemented yet");

            Console.ReadKey();
        }

        public void TradeOption()
        {
            Console.Clear();
            Console.WriteLine("whoopsies, this hasnt been implemented yet");

            Console.ReadKey();
        }

        public void ViewProfileOption()
        {
            Console.Clear();
            Console.WriteLine("whoopsies, this hasnt been implemented yet");

            Console.ReadKey();
        }
    }
}
