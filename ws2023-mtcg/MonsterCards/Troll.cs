using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.MonsterCards
{
    internal class Troll : Cards
    {
        public Troll() : base("Troll", ElementType.fire, CardType.monster, 15)
        {

        }

        public override Cards Attack(Cards target)
        {
            // trolls always win to normal enemies
            if(target._element == ElementType.normal)
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
