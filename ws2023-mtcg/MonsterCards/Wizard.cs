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
            if (target.Name == "Ork")
            {
                Console.WriteLine($"{this.Name} took control of {target.Name}! {this.Name} defeats {target.Name}!");

                target.IsAlive = false;
                return this;
            }

            // wizard always loses to firespells (which actually doenst make sense considering wizards are water types but shush)
            if (target.Name == "FireSpell")
            {
                Console.WriteLine($"{this.Name}'s robes are very flammable! {target.Name} defeats {this.Name}!");

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
