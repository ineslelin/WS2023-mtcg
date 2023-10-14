using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.SpellCards
{
    internal class RegularSpell : Cards
    {
        public RegularSpell() : base("RegularSpell", ElementType.normal, CardType.spell, 15)
        {

        }

        public override Cards Attack(Cards target)
        {
            // orks are immune to regular spells
            if (target._name == "Ork")
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
