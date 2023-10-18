using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.MonsterCards
{
    internal class Wizard : Cards
    {
        public Wizard() : base("Wizard", ElementType.water, CardType.monster)
        {

        }

        public override Cards Attack(Cards target)
        {
            // wizards always win against orks
            if (target._name == "Ork")
            {
                Console.WriteLine($"{this._name} took control of {target._name}! {this._name} defeats {target._name}!");
                return this;
            }

            // wizard always loses to firespells (which actually doenst make sense considering wizards are water types but shush)
            if (target._name == "FireSpell")
            {
                Console.WriteLine($"{this._name}'s robes are very flammable! {target._name} defeats {this._name}!");
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
