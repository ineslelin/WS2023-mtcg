using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg
{
    internal abstract class Cards
    {
        public string _name { get; set; }
        public ElementType _element { get; set; }
        public CardType _type { get; set; }
        public int _damage { get; set; }
        public bool IsAlive;

        public Cards(string _name, ElementType _element, CardType _type, int _damage)
        {
            this._name = _name;
            this._element = _element;
            this._type = _type;
            this._damage = _damage;

            IsAlive = true;
        }

        public abstract Cards Attack(Cards target);

        public Cards DamageFight(Cards source, Cards target)
        {
            if (source._damage > target._damage)
            {
                target.IsAlive = false;
                return source;
            }

            source.IsAlive = false;
            return target;
        }

        public Cards ElementFight(Cards source, Cards target)
        {
            if(source._element == target._element)
            {
                return DamageFight(source, target);
            }

            if((source._element == ElementType.water && target._element == ElementType.fire) ||
               (source._element == ElementType.fire && target._element == ElementType.normal) ||
               (source._element == ElementType.normal && target._element == ElementType.water)) 
            {
                if(source._damage / 2 > target._damage * 2)
                {
                    target.IsAlive = false;
                    return source;
                }

                source.IsAlive = false;
                return target;
            }

            if (target._damage / 2 > source._damage * 2)
            {
                source.IsAlive = false;
                return target;
            }

            target.IsAlive = false;
            return source;
        }
    }
}
