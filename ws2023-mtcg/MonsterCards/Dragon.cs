using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ws2023_mtcg.MonsterCards
{
    internal class Dragon : Cards
    {
        public Dragon() : base("Dragon", ElementType.fire, CardType.monster)
        {

        }

        public override Cards Attack(Cards target)
        {
            // dragon always wins against goblin
            if (Regex.IsMatch(target.Name, @"(Fire|Water|Regular)?Goblin"))
            {
                Console.WriteLine($"{target.Name} is too afraid of {this.Name} to attack! {this.Name} defeats {target.Name}!");

                target.IsAlive = false;
                return this;
            }

            // elf always defeats dragon
            if (Regex.IsMatch(target.Name, @"(Fire|Water|Regular)?Elf"))
            {
                Console.WriteLine($"Due to their age-old acquaintace {target.Name} knows how to evade all of {this.Name}'s attacks! " +
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
