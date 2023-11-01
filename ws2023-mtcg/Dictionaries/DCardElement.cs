using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Enums;

namespace ws2023_mtcg.Dictionaries
{
    internal class DCardElement
    {
        public DCardElement()
        {

        }

        public Dictionary<MonsterType, ElementType> MonsterCardElement = new Dictionary<MonsterType, ElementType>
        {
            // fire types
            { MonsterType.Dragon, ElementType.fire },
            { MonsterType.FireElf, ElementType.fire },
            { MonsterType.FireGoblin, ElementType.fire },
            { MonsterType.FireTroll, ElementType.fire },

            // regular types
            { MonsterType.RegularElf, ElementType.normal },
            { MonsterType.RegularGoblin, ElementType.normal },
            { MonsterType.RegularTroll, ElementType.normal },
            { MonsterType.Knight, ElementType.normal },
            { MonsterType.Ork, ElementType.normal },

            // water types
            { MonsterType.WaterElf, ElementType.water },
            { MonsterType.WaterGoblin, ElementType.water },
            { MonsterType.WaterTroll, ElementType.water },
            { MonsterType.Kraken, ElementType.water },
            { MonsterType.Wizard, ElementType.water },
        };

        public Dictionary<SpellType, ElementType> SpellCardElement = new Dictionary<SpellType, ElementType>
        {
            { SpellType.FireSpell, ElementType.fire },
            { SpellType.RegularSpell, ElementType.normal },
            { SpellType.WaterSpell, ElementType.water },
        };
    }
}
