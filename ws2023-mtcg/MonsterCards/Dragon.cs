using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.MonsterCards
{
    internal class Dragon : Cards
    {
        public Dragon() : base("Dragon", ElementType.fire, CardType.monster, 15)
        {

        }

        public override Cards Attack(Cards target)
        {
            // dragon always wins against goblin
            if (target._name == "Goblin")
            {
                return this;
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
