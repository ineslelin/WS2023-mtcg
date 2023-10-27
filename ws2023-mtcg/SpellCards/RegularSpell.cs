using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.SpellCards
{
    internal class RegularSpell : Cards
    {
        public RegularSpell() : base("RegularSpell", ElementType.normal, CardType.spell)
        {

        }

        public override Cards Attack(Cards target)
        {
            // orks are immune to regular spells
            if (target.Name == "Ork")
            {
                Console.WriteLine($"{target.Name} defeats {this.Name}!");
                return target;
            }

            // kraken is immune to spells
            if (target.Name == "Kraken")
            {
                Console.WriteLine($"Spells don't affect {target.Name}, rendering {this.Name} useless! " +
                    $"{target.Name} defeats {this.Name}!");
                return target;
            }

            // trolls always win to normal enemies
            if (this.Element == ElementType.normal && target.Name == "Troll")
            {
                Console.WriteLine($"Regular attacks and spell don't affect {target.Name}! {target.Name} defeats {this.Name}!");
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
