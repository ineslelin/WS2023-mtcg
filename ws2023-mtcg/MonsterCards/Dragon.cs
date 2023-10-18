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
            if (Regex.IsMatch(target._name, @"(Fire|Water|Regular)?Goblin"))
            {
                Console.WriteLine($"{target._name} is too afraid of {this._name} to attack! {this._name} defeats {target._name}!");
                return this;
            }

            // elf always defeats dragon
            if (Regex.IsMatch(target._name, @"(Fire|Water|Regular)?Elf"))
            {
                Console.WriteLine($"Due to their age-old acquaintace {target._name} knows how to evade all of {this._name}'s attacks! " +
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
