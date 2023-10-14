using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.SpellCards
{
    internal class WaterSpell : Cards
    {
        public WaterSpell() : base("WaterSpell", ElementType.water, CardType.spell, 10)
        {

        }

        public override Cards Attack(Cards target)
        {
            // knight always loses against waterspell
            if (target._name == "Knight")
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
