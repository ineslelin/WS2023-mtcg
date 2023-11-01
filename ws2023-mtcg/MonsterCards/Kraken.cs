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
            if (target.Type == CardType.spell)
            {
                Console.WriteLine($"Spells don't affect {this.Name}, rendering {target.Name} useless! " +
                    $"{this.Name} defeats {target.Name}!");

                target.IsAlive = false;
                return this;
            }

            return DamageFight(this, target);
        }
    }
}
