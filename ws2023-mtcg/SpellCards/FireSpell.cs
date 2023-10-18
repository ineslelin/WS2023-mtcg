using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.SpellCards
{
    internal class FireSpell : Cards
    {
        public FireSpell() : base("FireSpell", ElementType.fire, CardType.spell)
        {

        }

        public override Cards Attack(Cards target)
        {
            // wizard always loses to firespells (which actually doenst make sense considering wizards are water types but shush)
            if (target._name == "Wizard")
            {
                Console.WriteLine($"{target._name}'s robes are very flammable! {this._name} defeats {target._name}!");
                return this;
            }

            // kraken is immune to spells
            if (target._name == "Kraken")
            {
                Console.WriteLine($"Spells don't affect {target._name}, rendering {this._name} useless! " +
                    $"{target._name} defeats {this._name}!");
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
