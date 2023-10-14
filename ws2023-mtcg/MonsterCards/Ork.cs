using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.MonsterCards
{
    internal class Ork : Cards
    {
        public Ork() : base("Ork", ElementType.normal, CardType.monster, 15)
        {

        }

        public override Cards Attack(Cards target)
        {
            // orks always lose to wizards
            if (target._name == "Wizard")
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
