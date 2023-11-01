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
            if (target.Name == "Wizard")
            {
                Console.WriteLine($"{target.Name}'s robes are very flammable! {this.Name} defeats {target.Name}!");

                target.IsAlive = false;
                return this;
            }

            // kraken is immune to spells
            if (target.Name == "Kraken")
            {
                Console.WriteLine($"Spells don't affect {target.Name}, rendering {this.Name} useless! " +
                    $"{target.Name} defeats {this.Name}!");

                this.IsAlive = false;
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
