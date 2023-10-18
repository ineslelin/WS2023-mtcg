using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.MonsterCards
{
    internal abstract class Goblin : Cards
    {
        public Goblin(string _name, ElementType _element, CardType _type) : base(_name, _element, _type)
        {

        }

        public override Cards Attack(Cards target)
        {
            // goblins always loose to dragons
            if (target._name == "Dragon")
            {
                Console.WriteLine($"{this._name} is too afraid of {target._name} to attack! {target._name} defeats {this._name}!");
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
