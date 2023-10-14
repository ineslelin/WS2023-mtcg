using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.MonsterCards
{
    internal class Goblin : Cards
    {
        public Goblin() : base("Goblin", ElementType.water, CardType.monster, 10)
        {

        }

        public override Cards Attack(Cards target)
        {
            // goblins always loose to dragons
            if (target._name == "Dragon")
            {
                return target;
            }

            switch (target._type)
            {
                case CardType.monster: return DamageFight(this, target);
                case CardType.spell: return ElementFight(this, target);
                default: return null;
            }
        }
    }
}
