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
            if (target.Name == "Wizard")
            {
                Console.WriteLine($"{target.Name} took control of {this.Name}! {target.Name} defeats {this.Name}!");
                return target;
            }

            // trolls always win to normal enemies
            if (target.Name == "Troll")
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
