using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.SpellCards
{
    internal class FireSpell : Cards
    {
        public FireSpell() : base("FireSpell", ElementType.fire, CardType.spell, 20)
        {

        }

        public override Cards Attack(Cards target)
        {
            // wizard always loses to firespells (which actually doenst make sense considering wizards are water types but shush)
            if (target._name == "Wizard")
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
