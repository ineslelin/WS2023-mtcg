using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.MonsterCards
{
    internal class Wizard : Cards
    {
        public Wizard() : base("Wizard", ElementType.water, CardType.monster, 10)
        {

        }

        public override Cards Attack(Cards target)
        {
            // wizards always win against orks
            if (target._name == "Ork")
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
