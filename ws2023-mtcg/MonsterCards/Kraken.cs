using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.MonsterCards
{
    internal class Kraken : Cards
    {
        public Kraken() : base("Kraken", ElementType.water, CardType.monster)
        {

        }

        public override Cards Attack(Cards target)
        {
            // the kraken is immune to spells
            if (target._type == CardType.spell)
            {
                Console.WriteLine($"Spells don't affect {this._name}, rendering {target._name} useless! " +
                    $"{this._name} defeats {target._name}!");
                return this;
            }

            return DamageFight(this, target);
        }
    }
}
