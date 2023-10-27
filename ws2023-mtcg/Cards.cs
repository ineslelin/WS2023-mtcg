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

        DElementDependency elementDependency = new DElementDependency();

        public Cards(string _name, ElementType _element, CardType _type)
        {
            this._name = _name;
            this._element = _element;
            this._type = _type;

            Random random = new Random();
            this._damage = random.Next(5, 100);

            IsAlive = true;
        }

        public abstract Cards Attack(Cards target);

        public Cards DamageFight(Cards source, Cards target)
        {
            if (source._damage > target._damage)
            {
                target.IsAlive = false;
                AnnounceWinner(source, target);

                return source;
            }

            source.IsAlive = false;
            AnnounceWinner(source, target);

            return target;
        }

        public Cards ElementFight(Cards source, Cards target)
        {
            if(source._element == target._element)
            {
                return DamageFight(source, target);
            }

            //if((source._element == ElementType.water && target._element == ElementType.fire) ||
            //   (source._element == ElementType.fire && target._element == ElementType.normal) ||
            //   (source._element == ElementType.normal && target._element == ElementType.water)) 
            if (source._element == elementDependency.ElementDependencies[Tuple.Create(source._element, target._element)])
            {
                if(source._damage / 2 > target._damage * 2)
                {
                    target.IsAlive = false;
                    AnnounceWinner(source, target);

                    return source;
                }  

                source.IsAlive = false;
                AnnounceWinner(source, target);

                return target;
            }

            if (target._damage / 2 > source._damage * 2)
            {
                source.IsAlive = false;
                AnnounceWinner(source, target);

                return target;
            }

            target.IsAlive = false;
            AnnounceWinner(source, target);

            return source;
        }

        public void AnnounceWinner(Cards source, Cards target)
        {
            switch(target._type)
            {
                case CardType.monster when source._type == CardType.monster:
                    Console.WriteLine(source._damage + " vs " + target._damage + " => " + (source._damage > target._damage ? source._name : target._name)
                        + " defeats " + (source._damage < target._damage ? source._name : target._name) + "!");
                    break;

                case CardType.monster when source._type == CardType.spell:
                case CardType.spell when source._type == CardType.monster:
                case CardType.spell when source._type == CardType.spell:
                    if(source._element == target._element)
                    {
                        Console.WriteLine(source._damage + " vs " + target._damage + " => " + (source._damage > target._damage ? source._name : target._name)
                        + " defeats " + (source._damage < target._damage ? source._name : target._name) + "!");
                        break;
                    }

                    Console.WriteLine(source._damage + " vs " + target._damage + " => ");
                    break;
            }
        }
    }
}

