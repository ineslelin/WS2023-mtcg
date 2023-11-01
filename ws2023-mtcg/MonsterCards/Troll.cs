using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.MonsterCards
{
    internal abstract class Troll : Cards
    {
        public Troll(string _name, ElementType _element, CardType _type) : base(_name, _element, _type)
        {

        }

        public override Cards Attack(Cards target)
        {
            // trolls always win to normal enemies
            if(target.Element == ElementType.normal)
            {
                Console.WriteLine($"Regular attacks and spell don't affect {this.Name}! {this.Name} defeats {target.Name}!");

                target.IsAlive = false;
                return this;
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
