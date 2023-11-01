using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.MonsterCards
{
    internal abstract class Elf : Cards
    {
        public Elf(string _name, ElementType _element, CardType _type) : base(_name, _element, _type)
        {

        }

        public override Cards Attack(Cards target)
        {
            // elves always win agaisnt dragons
            if (target.Name == "Dragon")
            {
                Console.WriteLine($"Due to their age-old acquaintace {this.Name} knows how to evade all of {target.Name}'s attacks! " +
                    $"{this.Name} defeats {target.Name}!");

                target.IsAlive = false;
                return this;
            }

            // trolls always win to normal enemies
            if (this.Element == ElementType.normal && target.Name == "Troll")
            {
                Console.WriteLine($"Regular attacks and spell don't affect {target.Name}! {target.Name} defeats {this.Name}!");

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
