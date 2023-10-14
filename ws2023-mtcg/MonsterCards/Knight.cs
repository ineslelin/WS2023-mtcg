using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.MonsterCards
{
    internal class Knight : Cards
    {
        public Knight() : base("Knight", ElementType.normal, CardType.monster, 10)
        {

        }

        public override Cards Attack(Cards target)
        {
            // knights always lose to waterspells
            if (target._name == "WaterSpell")
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
