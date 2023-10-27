using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.SpellCards
{
    internal class WaterSpell : Cards
    {
        public WaterSpell() : base("WaterSpell", ElementType.water, CardType.spell)
        {

        }

        public override Cards Attack(Cards target)
        {
            // knight always loses against waterspell
            if (target.Name == "Knight")
            {
                Console.WriteLine($"{target.Name} drowned from {this.Name}'s attack! {this.Name} defeats {target.Name}!");
                return this;
            }

            // kraken is immune to spells
            if (target.Name == "Kraken")
            {
                Console.WriteLine($"Spells don't affect {target.Name}, rendering {this.Name} useless! " +
                    $"{target.Name} defeats {this.Name}!");
                return target;
            }

            switch (target.Type)
            {
                case CardType.monster: return DamageFight(this, target);
                case CardType.spell: return ElementFight(this, target);
            }

            // ok this is literally only for now, afterwards theres gonna be a catch here okok
            return this;
        }
    }
}
