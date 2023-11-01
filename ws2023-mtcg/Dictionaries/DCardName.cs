using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Enums;

namespace ws2023_mtcg.Dictionaries
{
    internal class DCardName
    {
        public DCardName()
        {

        }

        public Dictionary<MonsterType, string> MonsterName = new Dictionary<MonsterType, string>()
        {
            { MonsterType.FireElf, "FireElf" },
            { MonsterType.RegularElf, "RegularElf" },
            { MonsterType.WaterElf, "WaterElf" },
            { MonsterType.FireGoblin, "FireGoblin" },
            { MonsterType.RegularGoblin, "RegularGoblin" },
            { MonsterType.WaterGoblin, "WaterGoblin" },
            { MonsterType.FireTroll, "FireTroll" },
            { MonsterType.RegularTroll, "RegularTroll" },
            { MonsterType.WaterTroll, "WaterTroll" },
            { MonsterType.Dragon, "Dragon" },
            { MonsterType.Knight, "Knight" },
            { MonsterType.Kraken, "Kraken" },
            { MonsterType.Ork, "Ork" },
            { MonsterType.Wizard, "Wizard" },
        };

        public Dictionary<SpellType, string> SpellName = new Dictionary<SpellType, string>()
        {
            { SpellType.FireSpell, "FireSpell" },
            { SpellType.RegularSpell, "RegularSpell" },
            { SpellType.WaterSpell, "WaterSpell" },
        };
    }
}
