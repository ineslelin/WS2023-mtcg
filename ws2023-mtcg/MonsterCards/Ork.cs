using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.MonsterCards
{
    internal class Ork : Cards
    {
        public Ork() : base("Ork", ElementType.normal, CardType.monster)
        {

        }

        public override Cards Attack(Cards target)
        {
            // orks always lose to wizards
            if (target._name == "Wizard")
            {
                Console.WriteLine($"{target._name} took control of {this._name}! {target._name} defeats {this._name}!");
                return target;
            }

            // trolls always win to normal enemies
            if (target._name == "Troll")
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
