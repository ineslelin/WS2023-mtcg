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
            if (target._name == "Ork")
            {
                Console.WriteLine($"{target._name} defeats {this._name}!");
                return target;
            }

            // kraken is immune to spells
            if (target._name == "Kraken")
            {
                Console.WriteLine($"Spells don't affect {target._name}, rendering {this._name} useless! " +
                    $"{target._name} defeats {this._name}!");
                return target;
            }

            // trolls always win to normal enemies
            if (this._element == ElementType.normal && target._name == "Troll")
            {
                Console.WriteLine($"Regular attacks and spell don't affect {target._name}! {target._name} defeats {this._name}!");
                return target;
            }

            switch (target._type)
            {
                case CardType.monster: return DamageFight(this, target);
                case CardType.spell: return ElementFight(this, target);
            }

            // ok this is literally only for now, afterwards theres gonna be a catch here okok
            return this;
        }
    }
}
